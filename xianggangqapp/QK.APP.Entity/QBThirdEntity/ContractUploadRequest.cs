using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 文件上传接口参数
    /// </summary>
    public class ContractUploadRequest
    {
        /// <summary>
        /// 接入系统ID
        /// </summary>
        public string APP_ID { get; set; }
        /// <summary>
        /// 值为CONT_UPLOAD
        /// </summary>
        public string ACTION { get; set; }
        public _BIZ_INFO BIZ_INFO { get; set; }
        public class _BIZ_INFO
        {
            /// <summary>
            /// 申请的业务基本信息
            /// </summary>
            public List<BIZ_KEY_VAL> BASE_INFO { get; set; }
            public _FILE FILE { get; set; }
        }
    }
    public class _FILE
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public string FILE_ID { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FILE_TITLE { get; set; }
        /// <summary>
        /// 文件格式
        /// </summary>
        public string FILE_TYPE { get; set; }
        /// <summary>
        /// 文件二进制流
        /// </summary>
        public byte[] FILE_CONTENT { get; set; }
    }
}

