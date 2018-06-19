using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 合同手动签章请求参数
    /// </summary>
    public class ContractManualSign
    {
        /// <summary>
        /// 接入系统ID
        /// </summary>
        public string APP_ID { get; set; }
        /// <summary>
        /// 接口ACTION参数MANUAL_SIGN
        /// </summary>
        public string ACTION { get; set; }
        /// <summary>
        /// 手动签章完成后，浏览器则会自动跳转到此页面UR
        /// 注：目前对接的法大大平台请求方式为HTTP GET
        /// </summary>
        public string RETURN_URL { get; set; }

        public _BIZ_INFO BIZ_INFO { get; set; }
        public CA_INFO CA_INFO { get; set; }
        public class _BIZ_INFO
        {
            public List<BIZ_KEY_VAL> BASE_INFO { get; set; }
            public _FILESign FILE { get; set; }
        }
        public class _FILESign
        {
            public string FILE_ID { get; set; }
            public string FILE_TITLE { get; set; }
            public string FILE_TYPE { get; set; }
            public byte[] FILE_CONTENT { get; set; }
            public string FILE_SIGN_KEY { get; set; }
        }





    }


}
