/*********************
 * 作者：刘成帅
 * 时间：2014/9/10
 * 功能：时间选择字段
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
    public class DatePicker : AFieldType
    {
        public override string StringTemplatePath { get { return GlobalSetting.DFormPath + @"\DatePicker.html"; } }

        [DisplayName("日期格式")]
        public string Format { get; set; }

        [DisplayName("是否只读(1-是,0-否)")]
        public bool IsReadOnly { get; set; }

        [DisplayName("是否限制最大可选择日期为今天(1-是,0-否)")]
        public bool IsLimitDate { get; set; }
        public DatePicker(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);
            if (dic.ContainsKey("Format"))
            {
                this.Format = dic["Format"];
            }
            if (dic.ContainsKey("IsReadOnly"))
            {
                this.IsReadOnly = dic["IsReadOnly"].ToBoolean();
            }
            else
            {
                this.IsReadOnly = false;
            }
            if (dic.ContainsKey("IsLimitDate"))
            {
                this.IsLimitDate = dic["IsLimitDate"].ToBoolean();
            }
            else
            {
                this.IsLimitDate = false;
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
