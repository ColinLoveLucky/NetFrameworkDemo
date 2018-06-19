/*********************
 * 作    者:刘成帅
 * 创建时间:2014/9/9
 * 功    能:格式工厂，用来创建不同类型的对象以及连接对象成为可用的HTML
**********************/
using Antlr3.ST;
using QK.QAPP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.MvcScaffold.DForm
{
    /// <summary>
    /// 格式工厂，用来创建不同类型的对象以及连接对象成为可用的HTML
    /// </summary>
    public static class FieldTypeFactory
    {
        /// <summary>
        /// 根据类型创建基类对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldID"></param>
        /// <returns></returns>
        public static AFieldType CreateFieldType(string type, long fieldID)
        {

            switch (type)
            {
                case "Text":
                    return new Text(fieldID);
                case "Radio":
                    return new Radio(fieldID);
                case "Select":
                    return new Select(fieldID);
                case "TelNumber":
                    return new TelNumber(fieldID);
                case "Number":
                    return new Number(fieldID);
                case "DatePicker":
                    return new DatePicker(fieldID);
                case "StaffPicker":
                    return new StaffPicker(fieldID);
                case "Adress":
                    return new Adress(fieldID);
                case "FileViewer":
                    return new FileViewer(fieldID);
                case "Other":
                    return new Other(fieldID);
                case "PrimaryKey":
                    return new PrimaryKey(fieldID);
                case "TextArea":
                    return new TextArea(fieldID);
                case "CompanyPicker":
                    return  new CompanyPicker(fieldID);
                case "CarPicker":
                    return new CarPicker(fieldID);
                case "CarAgencyPicker":
                    return new CarAgencyPicker(fieldID);
                default:
                    return new Text(fieldID);
                //throw new ArgumentException("参数错误：未读取到相对应类型");
            }
        }
        /// <summary>
        /// 拼接字段
        /// </summary>
        /// <param name="list"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public static string FieldJoiner(List<AFieldType> list, bool readOnly)
        {
            string StringTemplatePath = GlobalSetting.DFormPath + @"\DFromContainer\GroupContainer.html";
            list = list.Where(c => c.Field_RowSpan != 0).OrderBy(c => c.Field_Sort).ToList();
            List<string> groupField = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                if (i + 1 < list.Count)
                {
                    var current = list[i];
                    var next = list[i + 1];
                    if (current.Field_RowSpan == 1 && next.Field_RowSpan == 1)
                    {
                        groupField.Add(current.GetHTML(readOnly) + next.GetHTML(readOnly));
                        i++;
                    }
                    else
                    {
                        groupField.Add(current.GetHTML(readOnly));
                    }
                }
                else
                {
                    groupField.Add(list[i].GetHTML(readOnly));
                }
            }
            foreach (var item in list.Where(c => c.Field_RowSpan == 0))
            {
                groupField.Add(item.GetHTML(readOnly));
            }
            var template = FileReader.GetTmpString(StringTemplatePath);
            StringTemplate st = new StringTemplate(template);
            st.SetAttribute("Field", groupField);
            return st.ToString();
        }
    }
}
