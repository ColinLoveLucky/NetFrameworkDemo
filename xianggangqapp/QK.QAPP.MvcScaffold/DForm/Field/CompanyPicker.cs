using Antlr3.ST;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QK.QAPP.MvcScaffold.DForm
{
    public class CompanyPicker : AFieldType
    {
        public override string StringTemplatePath
        {
            get { return GlobalSetting.DFormPath + @"\CompanyPicker.html"; }
        }

        #region 控件属性
        [DisplayName("正则表达式")]
        public string Regular { get; set; }

        [DisplayName("最大长度写数字(默认50)")]
        public string MaxLength { get; set; }

        [DisplayName("字典表中企业类型")]
        public string MapDICCode { get; set; }

        [DisplayName("加载企业列表所调用的方法（GetCompanyList或ListByCategory）")]
        public string CompanyFunc { get; set; }

        [DisplayName("企业类型为其他时的Type")]
        public string OtherType { get; set; }

        public bool HasRegular { get { return !string.IsNullOrEmpty(this.Regular); } }

        [DisplayName("对应数据库表中存储企业类型的字段名")]
        public string Mapper_AEO_Type { get; set; }

        [DisplayName("对应数据库表中存储公司Code的字段名")]
        public string Mapper_AEO_Code { get; set; }

        [DisplayName("未找到数据时的提示信息")]
        public string TipMessage { get; set; }
        #endregion

        //初始化
        public CompanyPicker(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);

            if (dic.ContainsKey("Regular"))
            {
                this.Regular = dic["Regular"] + "";
            }
            if (dic.ContainsKey("MaxLength") && !string.IsNullOrEmpty(dic["MaxLength"]))
            {
                this.MaxLength = dic["MaxLength"] + "";
            }
            else
            {
                this.MaxLength = "50";
            }

            if (dic.ContainsKey("MapDICCode"))
            {
                this.MapDICCode = dic["MapDICCode"];
            }

            if (dic.ContainsKey("CompanyFunc"))
            {
                this.CompanyFunc = dic["CompanyFunc"];
            }

            if (dic.ContainsKey("OtherType"))
            {
                this.OtherType = dic["OtherType"];
            }

            if (dic.ContainsKey("Mapper_AEO_Type"))
            {
                this.Mapper_AEO_Type = dic["Mapper_AEO_Type"];
            }
            if (dic.ContainsKey("Mapper_AEO_Code"))
            {
                this.Mapper_AEO_Code = dic["Mapper_AEO_Code"];
            }
            if (dic.ContainsKey("TipMessage"))
            {
                this.TipMessage = dic["TipMessage"];
            }
            else
            {
                this.TipMessage = "对不起，未找到符合条件的记录！";
            }

        }


        public override string GetHTML(bool readOnly)
        {
            var dicService = Ioc.GetService<ICR_DATA_DICService>();
            var dicList = dicService.GetDICByParentCode(this.MapDICCode);

            var template = FileReader.GetTmpString(this.StringTemplatePath);
            StringTemplate st = new StringTemplate(template);
            st.SetAttribute("FieldType", this);
            st.SetAttribute("ReadOnly", readOnly);
            st.SetAttribute("DIC", dicList);

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

            //长度
            if (!string.IsNullOrEmpty(value) && value.Length > this.MaxLength.ToInt())
            {
                error += string.Format(errorTemp, this.Field_Group, this.Field_DisplayName, "字符数超出限制");
                return false;
            }

            //正则
            if (this.HasRegular && !string.IsNullOrEmpty(value))
            {
                Regular = Regular.Replace("\\\\", "\\");
                Regex regex = new Regex(this.Regular);
                if (!regex.IsMatch(value.Trim()))
                {
                    error += string.Format(errorTemp, this.Field_Group, this.Field_DisplayName, "格式填写不正确");
                    return false;
                }
            }

            return true;
        }
    }
}
