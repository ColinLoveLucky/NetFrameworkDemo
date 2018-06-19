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
    public class Adress : AFieldType
    {
        public override string StringTemplatePath
        {
            get { return GlobalSetting.DFormPath + @"\Adress.html"; }
        }

        [DisplayName("区域代码（例如：AREA_CODE）")]
        public string MapDICCode { get; set; }

        //省份 id name
        [DisplayName("省份（后缀为：_Province）")]
        public string Province { get; set; }

        //市 id name
        [DisplayName("市（后缀为：_City）")]
        public string City { get; set; }

        //详细 id name
        [DisplayName("详细（后缀为：_Detail）")]
        public string Detail { get; set; }

        [DisplayName("关联表中省的字段名")]
        public string Mapper_Province { get; set; }
        [DisplayName("关联表中市的字段名")]
        public string Mapper_City { get; set; }
        [DisplayName("关联表中详细信息的字段名")]
        public string Mapper_Detail { get; set; }


        //页面异步加载市所请求的行为
        [DisplayName("页面异步加载所请求的url")]
        public string Action_Adress { get; set; }

        [DisplayName("是否隐藏搜索按钮")]
        public bool IsHiddenSearch { get; set; }
        [DisplayName("详细地址长度")]
        public int DetialLength { get; set; }
       

        public Adress(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);

            if (dic.ContainsKey("Mapper_Province"))
            {
                this.Mapper_Province = dic["Mapper_Province"];
            }
            if (dic.ContainsKey("Mapper_City"))
            {
                this.Mapper_City = dic["Mapper_City"];
            }
            if (dic.ContainsKey("Mapper_Detail"))
            {
                this.Mapper_Detail = dic["Mapper_Detail"];
            }

            if (dic.ContainsKey("MapDICCode"))
            {
                this.MapDICCode = dic["MapDICCode"];
            }

            if (dic.ContainsKey("Province"))
            {
                this.Province = dic["Province"];
            }

            if (dic.ContainsKey("City"))
            {
                this.City = dic["City"];
            }

            if (dic.ContainsKey("Action_Adress"))
            {
                this.Action_Adress = dic["Action_Adress"];
            }

            if (dic.ContainsKey("Detail"))
            {
                this.Detail = dic["Detail"];
            }
            if (dic.ContainsKey("IsHiddenSearch"))
            {
                this.IsHiddenSearch = dic["IsHiddenSearch"].ToBoolean();
            }
            if (dic.ContainsKey("DetialLength") && !string.IsNullOrEmpty(dic["DetialLength"]))
            {
                this.DetialLength = dic["DetialLength"].ToInt();
            }
            else
            {
                this.DetialLength = 50;
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
