using Antlr3.ST;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.ComponentModel;

namespace QK.QAPP.MvcScaffold.DForm
{
    public class Radio : AFieldType
    {
        public override string StringTemplatePath { get { return GlobalSetting.DFormPath + @"\Radio.html"; } }

        /// <summary>
        /// 匹配字典表里面的代码
        /// </summary>
        [DisplayName("是否为手机号")]
        public string MapDICCode { get; set; }

        public Radio(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);
            if (dic.ContainsKey("MapDICCode"))
            {
                this.MapDICCode = dic["MapDICCode"] + "";
            }
        }

        public override string GetHTML(bool readOnly)
        {
            var dicService = Ioc.GetService<ICR_DATA_DICService>();
            var dicList = dicService.GetDICByParentCode(this.MapDICCode);


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
