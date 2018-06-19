
/*********************
 * 作    者：刘成帅
 * 创建时间：2014/9/25
 * 功    能：选择类型
**********************/
using Antlr3.ST;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QK.QAPP.MvcScaffold.DForm
{
    public class Select : AFieldType
    {


        public override string StringTemplatePath { get { return GlobalSetting.DFormPath + @"\Select.html"; } }

        /// <summary>
        /// 匹配字典表里面的代码
        /// </summary>
        [DisplayName("匹配字典表中的代码，需要到数据库里面查询")]
        public string MapDICCode { get; set; }

        /// <summary>
        /// 匹配字典表里面的代码
        /// </summary>
        [DisplayName("如果是不是从字典表里面取数据需要填写，取数据表名")]
        public string Map_Table { get; set; }

        /// <summary>
        /// 匹配字典表里面的代码
        /// </summary>
        [DisplayName("如果是不是从字典表里面取数据需要填写，取数据字段主键名")]
        public string Map_TableKeyField { get; set; }

        /// <summary>
        /// 匹配字典表里面的代码
        /// </summary>
        [DisplayName("如果是不是从字典表里面取数据需要填写，取数据字段值名")]
        public string Map_TableValueField { get; set; }

        [DisplayName("如果是不是从字典表里面取数据需要填写，取数据WHERE 语句（如 Enable='1'）")]
        public string MAP_TableSQL_WHERE { get; set; }


        /// <summary>
        /// 匹配字典表里面的代码
        /// </summary>
        [DisplayName("仅仅只显示选中的值，用','隔开！")]
        public string CodeSelect { get; set; }

        [DisplayName("是否显示Code")]
        public bool IsDisplayCode { get; set; }

        [DisplayName("是否只读(1-是,0-否)")]
        public bool IsReadOnly { get; set; }

        public Select(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);
            if (dic.ContainsKey("MapDICCode"))
            {
                this.MapDICCode = dic["MapDICCode"] + "";
            }
            if (dic.ContainsKey("Map_Table"))
            {
                this.Map_Table = dic["Map_Table"] + "";
            }
            if (dic.ContainsKey("Map_TableKeyField"))
            {
                this.Map_TableKeyField = dic["Map_TableKeyField"] + "";
            }
            if (dic.ContainsKey("Map_TableValueField"))
            {
                this.Map_TableValueField = dic["Map_TableValueField"] + "";
            }
            if (dic.ContainsKey("MAP_TableSQL_WHERE"))
            {
                this.MAP_TableSQL_WHERE = dic["MAP_TableSQL_WHERE"] + "";
            }
            if (dic.ContainsKey("CodeSelect"))
            {
                this.CodeSelect = dic["CodeSelect"] + "";
            }
            if(dic.ContainsKey("IsDisplayCode"))
            {
                this.IsDisplayCode = dic["IsDisplayCode"].ToBoolean();
            }
            else
            {
                this.IsDisplayCode = false;
            }
            if (dic.ContainsKey("IsReadOnly"))
            {
                this.IsReadOnly = dic["IsReadOnly"].ToBoolean();
            }
            else
            {
                this.IsReadOnly = false;
            }
        }

        public override string GetHTML(bool readOnly)
        {
            var dicService = Ioc.GetService<ICR_DATA_DICService>();
            var dicList = dicService.GetDICByParentCode(this.MapDICCode);
            if (!string.IsNullOrEmpty(this.Map_Table))
            {
                string sql = string.Format("SELECT {0} as DATA_CODE,{1} as DATA_NAME FROM {2} WHERE {3}", this.Map_TableKeyField, this.Map_TableValueField, this.Map_Table, this.MAP_TableSQL_WHERE);
                dicList = dicService.SqlQuery<QK.QAPP.Entity.CR_DATA_DIC>(sql).ToList();

            }
            if (!string.IsNullOrEmpty(CodeSelect))
            {
                var selectList = CodeSelect.Split(',');
                dicList = dicList.Where(c => selectList.Any(d => d == c.DATA_NAME || d == c.DATA_CODE)).ToList();
            }
            var template = FileReader.GetTmpString(this.StringTemplatePath);
            StringTemplate st = new StringTemplate(template);
            st.SetAttribute("FieldType", this);
            st.SetAttribute("DIC", dicList);
            st.SetAttribute("ReadOnly", readOnly);

            return st.ToString();
        }

        public override bool Vaidate(string value, out string error)
        {
            var errorTemp = GlobalSetting.DFormErrorTemp;
            error = "";
            //判断必填
            if (this.Field_Required)
            {
                if (string.IsNullOrEmpty(value))
                {
                    error += string.Format(errorTemp, this.Field_Group, this.Field_DisplayName, "不能为空");
                    return false;
                }
            }
            return true;
        }
    }
}
