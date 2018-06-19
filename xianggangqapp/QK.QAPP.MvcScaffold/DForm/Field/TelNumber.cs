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
    public class TelNumber : AFieldType
    {
        public override string StringTemplatePath { get { return GlobalSetting.DFormPath + @"\TelNumber.html"; } }

        /// <summary>
        /// 是否是手机号
        /// </summary>
        [DisplayName("是否为手机号")]
        public bool PhoneNumber { get; set; }
        [DisplayName("号码格式（maskedinput支持的默认格式）")]
        public string NumberFormat { get; set; }

        public bool HasFormat { get { return !string.IsNullOrEmpty(NumberFormat); } }

        public TelNumber(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);
            if (dic.ContainsKey("PhoneNumber"))
            {
                this.PhoneNumber = (dic["PhoneNumber"] + "").ToBoolean();
            }
            if (dic.ContainsKey("NumberFormat"))
            {
                this.NumberFormat = dic["NumberFormat"];
            }
        }

        public override string GetHTML(bool readOnly)
        {
            var dicService = Ioc.GetService<ICR_DATA_DICService>();

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
            return true;
        }
    }
}
