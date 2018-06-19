using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public class LDER_INFO
    {
        /// <summary>
        /// 出借人姓名
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        /// 出借人夸客平台账号
        /// </summary>
        public string QF_USERID { get; set; }
        /// <summary>
        /// 出借人证件类型
        /// </summary>
        public string CERT_TYPE { get; set; }
        /// <summary>
        /// 出借人证件号码
        /// </summary>
        public string CERT_NO { get; set; }
        /// <summary>
        /// 出借人银行预留手机号/移动电话
        /// </summary>
        public string RESV_MOBL { get; set; }
        /// <summary>
        /// 出借人通讯地址
        /// </summary>
        public string USER_ADDR { get; set; }
        /// <summary>
        /// 出借人邮箱
        /// </summary>
        public string USER_MAIL { get; set; }
        /// <summary>
        /// 出借人户名=用户姓名
        /// </summary>
        public string BAND_DEPT_NAME { get; set; }
        /// <summary>
        /// 出借人银行账号
        /// </summary>
        public string BAND_DEPT_ACNT { get; set; }
        /// <summary>
        /// 出借人开户行
        /// </summary>
        public string BAND_DEPT { get; set; }
    }
}
