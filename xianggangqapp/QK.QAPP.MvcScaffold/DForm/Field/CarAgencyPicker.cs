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
    public class CarAgencyPicker : AFieldType
    {
        public override string StringTemplatePath
        {
            get { return GlobalSetting.DFormPath + @"\CarAgencyPicker.html"; }
        }

        #region 控件属性
        [DisplayName("正则表达式")]
        public string Regular { get; set; }

        [DisplayName("最大长度写数字(默认50)")]
        public string MaxLength { get; set; }
        public bool HasRegular { get { return !string.IsNullOrEmpty(this.Regular); } }

        [DisplayName("对应数据库表中的经销商Code的字段名")]
        public string Mapper_CarAgency_Code { get; set; }
        #endregion

        //初始化
        public CarAgencyPicker(long id)
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
            if (dic.ContainsKey("Mapper_CarAgency_Code"))
            {
                this.Mapper_CarAgency_Code = dic["Mapper_CarAgency_Code"];
            }
        }


        public override string GetHTML(bool readOnly)
        {
            var template = FileReader.GetTmpString(this.StringTemplatePath);
            StringTemplate st = new StringTemplate(template);
            st.SetAttribute("FieldType", this);
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

