using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.QBThirdEntity.ContractInfo
{
    /// <summary>
    /// 债权人信息
    /// </summary>
   public class DEBT_INFO
    {
        /// <summary>
        ///债权人 姓名
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        ///债权人 证件类型
        /// </summary>
        public string CERT_TYPE { get; set; }
        /// <summary>
        /// 债权人证件号码
        /// </summary>
        public string CERT_NO { get; set; }
        /// <summary>
        /// 债权人手机号/移动电话
        /// </summary>
        public string RESV_MOBL { get; set; }
        /// <summary>
        /// 债权人通讯地址
        /// </summary>
        public string USER_ADDR { get; set; }
        /// <summary>
        /// 债权人邮箱
        /// </summary>
        public string USER_MAIL { get; set; }
        /// <summary>
        /// 债权人传真
        /// </summary>
        public string FAX { get; set; }
    }
}
