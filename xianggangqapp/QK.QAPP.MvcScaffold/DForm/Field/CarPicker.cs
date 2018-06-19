using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Antlr3.ST;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;

namespace QK.QAPP.MvcScaffold.DForm
{
    public class CarPicker : AFieldType
    {
        public override string StringTemplatePath
        {
            get { return GlobalSetting.DFormPath + @"\CarPicker.html"; }
        }

        #region 控件属性
        
        [DisplayName("对应数据库表中的车辆品牌的字段名")]
        public string Mapper_Car_Brand { get; set; }

        [DisplayName("对应数据库表中的车辆系列的字段名")]
        public string Mapper_Car_Series { get; set; }

        [DisplayName("对应数据库表中的年款排量的字段名")]
        public string Mapper_Car_Style { get; set; }

        [DisplayName("对应数据库表中的车辆价格的字段名")]
        public string Mapper_Car_Price { get; set; }

        [DisplayName("对应数据库表中的生产年份的字段名")]
        public string Mapper_Car_Year { get; set; }

        [DisplayName("对应数据库表中的车辆信息类型字段名")]
        public string Mapper_CarInfo_Type { get; set; }

        [DisplayName("最大长度写数字(默认50)")]
        public string MaxLength { get; set; }

        [DisplayName("是否可自定义输入（1-是，0-否），默认为否")]
        public bool CanInputByCustom { get; set; }
        #endregion

        //初始化
        public CarPicker(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);

            if (dic.ContainsKey("Mapper_Car_Brand"))
            {
                this.Mapper_Car_Brand = dic["Mapper_Car_Brand"];
            }
            if (dic.ContainsKey("Mapper_Car_Series"))
            {
                this.Mapper_Car_Series = dic["Mapper_Car_Series"];
            }
            if (dic.ContainsKey("Mapper_Car_Style"))
            {
                this.Mapper_Car_Style = dic["Mapper_Car_Style"];
            }
            if (dic.ContainsKey("Mapper_Car_Price"))
            {
                this.Mapper_Car_Price = dic["Mapper_Car_Price"];
            }
            if (dic.ContainsKey("Mapper_Car_Year"))
            {
                this.Mapper_Car_Year = dic["Mapper_Car_Year"];
            }
            if (dic.ContainsKey("Mapper_CarInfo_Type"))
            {
                this.Mapper_CarInfo_Type = dic["Mapper_CarInfo_Type"];
            }

            if (dic.ContainsKey("MaxLength") && !string.IsNullOrEmpty(dic["MaxLength"]))
            {
                this.MaxLength = dic["MaxLength"] + "";
            }
            else
            {
                this.MaxLength = "50";
            }

            if (dic.ContainsKey("CanInputByCustom"))
            {
                this.CanInputByCustom = dic["CanInputByCustom"].ToBoolean();
            }
            else
            {
                this.CanInputByCustom = false;
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

            return true;
        }
    }
}
