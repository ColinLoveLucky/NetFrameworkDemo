using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 商户信息
    /// </summary>
   public class STORE_INFO
    {
        /// <summary>
        /// 户名
        /// </summary>
        public string BAND_DEPT_NAME { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BAND_DEPT_ACNT { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string BAND_DEPT { get; set; }
        /// <summary>
        /// 开户行支行
        /// </summary>
        public string BAND_DEPT_SUB { get; set; }
    }
}
