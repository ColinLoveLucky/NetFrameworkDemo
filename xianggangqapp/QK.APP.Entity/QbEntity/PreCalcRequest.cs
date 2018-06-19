using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class PreCalcRequest
    {
        /// <summary>
        /// 标的编号
        /// </summary>
        public string BidCode { get; set; }
        /// <summary>
        /// 还款日
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// 合同起息日
        /// </summary>
        public DateTime StarTime { get; set; }
        /// <summary>
        /// 合同结束日期
        /// </summary>
        public DateTime EndTime { get; set; }    
    }
}
