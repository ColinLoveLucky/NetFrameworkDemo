using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class FILE_LIST
    {
        /// <summary>
        /// 业务基本信息
        /// </summary>
        public List<BIZ_KEY_VAL> BASE_INFO { get; set; }
        /// <summary>
        /// 文件列表
        /// </summary>
        public List<Dictionary<string,FILE>> FILES { get; set; }
        public class FILE
        {
            /// <summary>
            /// 有公司电子签章和用户电子签章
            /// </summary>
            public FILEinfo E_AUTO { get; set; }
            /// <summary>
            /// 有公司电子签章，可通过SIGN_URL字段，打开三方平台界面，进行手动电子签章
            /// </summary>
            public FILEinfo E_MANUAL { get; set; }
            /// <summary>
            /// 有公司电子签章，可直接根据FILE_PATH下载并打印，给到用户签字再上传
            /// </summary>
            public FILEinfo P_MANUAL { get; set; }
            /// <summary>
            ///有公司电子签章和用户手动电子签章，为最终版
            /// </summary>
            public FILEinfo E_MANUAL_FINAL { get; set; }
            /// <summary>
            /// 有公司电子签章和用户手动签字
            /// </summary>
            public FILEinfo P_MANUAL_FINAL { get; set; }
            /// <summary>
            /// 没有公司签章和用户签章
            /// </summary>
            public FILEinfo P_NON_SEAL { get; set; }
        }

    }

    public class FILEinfo
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public string FILE_ID { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FILE_TITLE { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FILE_PATH { get; set; }
        /// <summary>
        /// 文件格式
        /// </summary>
        public string FILE_TYPE { get; set; }
        /// <summary>
        /// 手动签章地址
        /// </summary>
        public string SIGN_URL { get; set; }
    }
}
