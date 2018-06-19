using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using Antlr3.ST;
using System.ComponentModel;
using QK.QAPP.Entity;
using QK.QAPP.IServices;
using Microsoft.Practices.Unity;

namespace QK.QAPP.MvcScaffold.DForm
{
    /// <summary>
    /// 文件显示控件
    /// </summary>
    public class FileViewer : AFieldType
    {
        #region 属性

        /// <summary>
        /// 模板存放地
        /// </summary>

        public override string StringTemplatePath
        {
            get
            {
                return GlobalSetting.DFormPath + @"\FileViewer.html";
            }
        }

        /// <summary>
        /// 文件类型
        /// </summary>
        [DisplayName("文件类型的Code（区分大小写，来源：字典表中Data_Code为APPLY_INFO的子项的Data_Code值）")]
        public string FileType { get; set; }

        /// <summary>
        /// 获取文件列表的地址
        /// </summary>
        [DisplayName("获取文件列表的地址（如：/Fileupload/FileList）")]
        public string FileGetUrl { get; set; }

        /// <summary>
        /// 上传文件页面的地址
        /// </summary>
        [DisplayName("上传文件页面的地址（如：/Fileupload/index）")]
        public string FileUploadUrl { get; set; }

        /// <summary>
        /// 检查是否是需要补件的URL
        /// </summary>
        [DisplayName("检查是否是需要补件的URL（如：/Fileupload/CheckFileType）")]
        public string CheckSDUrl { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        [DisplayName("上传提示，如用于提示选择性上传（不要包含单引号）")]
        public string Tips { get; set; }

        /// <summary>
        /// ‘选择性上传’的同组成员
        /// </summary>
        [DisplayName("和哪些FileViewer控件一起构成‘选择性上传’。（包含自己。填写FileType，如：Id1,Id2）。此字段要求为必选。")]
        public string OptionalGroup { get; set; }

        /// <summary>
        /// ‘选择性上传’的个数
        /// </summary>
        [DisplayName("本组最少选择性上传几个控件")]
        public int OptionalCount { get; set; }

        /// <summary>
        /// 申请金额到达多少后必填
        /// </summary>
        [DisplayName("申请金额到达多少后必填。此字段要求为必选。")]
        public int NeedIfAmout { get; set; }

        int fileAmountAtleast = 0;

        [DisplayName("该类型至少上次几张，值大于零才会验证。")]
        public int FileAmountAtleast
        {
            get { return fileAmountAtleast; }
            set { fileAmountAtleast = value; }
        }

        public bool HasFileAmount { get { return FileAmountAtleast > 0; } }
        #endregion

        public FileViewer(long id)
            : base(id)
        {
            HttpContext context = System.Web.HttpContext.Current;

            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);
            if (dic.ContainsKey("FileType"))
            {
                FileType = dic["FileType"];
            }
            if (dic.ContainsKey("FileGetUrl"))
            {
                FileGetUrl = dic["FileGetUrl"];
            }
            if (dic.ContainsKey("FileUploadUrl"))
            {
                FileUploadUrl = dic["FileUploadUrl"];
            }
            if (dic.ContainsKey("CheckSDUrl"))
            {
                CheckSDUrl = dic["CheckSDUrl"];
            }
            if (dic.ContainsKey("OptionalGroup"))
            {
                OptionalGroup = dic["OptionalGroup"];
            }
            if (dic.ContainsKey("OptionalCount"))
            {
                OptionalCount = string.IsNullOrWhiteSpace(dic["OptionalCount"]) ? 0 : int.Parse(dic["OptionalCount"]);
            }
            if (dic.ContainsKey("NeedIfAmout"))
            {
                NeedIfAmout = string.IsNullOrWhiteSpace(dic["NeedIfAmout"]) ? -1 : int.Parse(dic["NeedIfAmout"]);
            }
            if (dic.ContainsKey("FileAmountAtleast"))
            {
                FileAmountAtleast = string.IsNullOrWhiteSpace(dic["FileAmountAtleast"]) ? 0 : int.Parse(dic["FileAmountAtleast"]);
            }

            //获取描述信息
            ICR_DATA_DICService DataDicService = Ioc.GetService<ICR_DATA_DICService>();
            CR_DATA_DIC dataDic = DataDicService.GetDICByCode(FileType);
            if (dataDic != null && !string.IsNullOrWhiteSpace(dataDic.DATA_DESC))
            {
                Tips = "<strong>提示：</strong>" + dataDic.DATA_DESC;
            }
            if (dic.ContainsKey("Tips") && !string.IsNullOrWhiteSpace(dic["Tips"]))
            {
                Tips += "<br /><strong>说明：</strong>" + dic["Tips"];
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
            return true;
        }
    }
}
