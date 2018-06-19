using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.QBThirdEntity.ContractInfo
{
    /// <summary>
    /// 保证人信息
    /// </summary>
    public class INDICATOR
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string CERT_NO { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string QIANMING { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string DATA { get; set; }
    }
}
