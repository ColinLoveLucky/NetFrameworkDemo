/*********************
 * 作    者：刘成帅
 * 创建时间：2014/9/25
 * 功    能：其他类型字段 纯粹是JS类型的字段
**********************/
using Antlr3.ST;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace QK.QAPP.MvcScaffold.DForm
{
    public class Other : AFieldType
    {
        [DisplayName("Other关联表单元素的ID")]
        public string Trigger_ID { get; set; }

        [DisplayName("Other关联表单元素的Value")]
        public string Trigger_Value { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        [DisplayName("前缀")]
        public string Prefix { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        [DisplayName("后缀")]
        public string Suffix { get; set; }

        public bool HasRegular { get { return !string.IsNullOrEmpty(this.Regular); } }

        /// <summary>
        /// 正则表达式
        /// </summary>
        [DisplayName("正则表达式")]
        public string Regular { get; set; }
        [DisplayName("最大长度（默认为50）")]
        public string MaxLength { get; set; }

        public override string StringTemplatePath { get { return GlobalSetting.DFormPath + @"\Other.html"; } }


        public Other(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);
            if (dic.ContainsKey("Trigger_ID"))
            {
                this.Trigger_ID = dic["Trigger_ID"] + "";
            }
            if (dic.ContainsKey("Trigger_Value"))
            {
                this.Trigger_Value = dic["Trigger_Value"] + "";
            }
            if (dic.ContainsKey("Prefix"))
            {
                this.Prefix = dic["Prefix"] + "";
            }
            if (dic.ContainsKey("Suffix"))
            {
                this.Suffix = dic["Suffix"] + "";
            }
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
            //长度
            if (!string.IsNullOrEmpty(value) && value.Length > this.MaxLength.ToInt())
            {
                error += string.Format(errorTemp, this.Field_Group, this.Field_DisplayName, "字符数超出限制");
                return false;
            }
            return true;
        }
    }
}
