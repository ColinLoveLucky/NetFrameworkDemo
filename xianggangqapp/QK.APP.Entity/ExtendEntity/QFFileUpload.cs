using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QFFileUpload
    {
        /// <summary>
        /// 申请单号
        /// </summary>
        public string bizCode { get; set; }

        /// <summary>
        /// 申请者证件号码
        /// </summary>
        public string idNo { get; set; }

        /// <summary>
        /// 申请者证件类型
        /// </summary>
        public string idType { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// 文件类型（如：身份证 或 工作证明 或 收入证明等）
        /// </summary>
        public string fileType { get; set; }

        /// <summary>
        /// 文件上传者
        /// </summary>
        public string loginUser { get; set; }

        /// <summary>
        /// 文件内容（注意大小限制）
        /// </summary>
        public byte[] fileContent { get; set; }
    }
}
