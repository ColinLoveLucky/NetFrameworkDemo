/*********************
 * 作    者：刘成帅
 * 创建时间：2014/9/18
 * 功    能：数值类型字段
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
    public class Number : AFieldType
    {
        public override string StringTemplatePath { get { return GlobalSetting.DFormPath + @"\Number.html"; } }


        /// <summary>
        /// 默认值
        /// </summary>
       [DisplayName("默认值")]
        public int DefaultValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        [DisplayName("最大值")]
        public int MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        [DisplayName("最小值")]
        public int MinValue { get; set; }

        /// <summary>
        /// 数值单位
        /// </summary>
        [DisplayName("数值单位")]
        public string Unit { get; set; }


        /// <summary>
        /// 步长
        /// </summary>
        [DisplayName("步长")]
        public int Step { get; set; }

        /// <summary>
        /// 最大位数
        /// </summary>
        [DisplayName("最大位数")]
        public int MaxBit { get; set; }


        public Number(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);
            if (dic.ContainsKey("MaxValue"))
            {
                this.MaxValue = dic["MaxValue"].ToInt();
            }
            if (dic.ContainsKey("MinValue"))
            {
                this.MinValue = dic["MinValue"].ToInt();
            }
            if (dic.ContainsKey("Unit"))
            {
                this.Unit = dic["Unit"] + "";
            }
            if (dic.ContainsKey("Step"))
            {
                this.Step = dic["Step"].ToInt();
            }
            if (dic.ContainsKey("DefaultValue"))
            {
                this.DefaultValue = dic["DefaultValue"].ToInt();
            }
            if(dic.ContainsKey("MaxBit"))
            {
                this.MaxBit = dic["MaxBit"].ToInt();
                if (this.MaxBit <= 0)
                    this.MaxBit = 100;
            }
            else
            {
                this.MaxBit = 100;
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
            //长度及范围
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > this.MaxBit)
                {
                    error += string.Format(errorTemp, this.Field_Group, this.Field_DisplayName, "字符数超出限制");
                    return false;
                }
                if (value.ToInt() > this.MaxValue || value.ToInt() < this.MinValue)
                {
                    error += string.Format(errorTemp, this.Field_Group, this.Field_DisplayName, "数值超出规定范围");
                    return false;
                }
            }

            return true;
        }
    }
}
