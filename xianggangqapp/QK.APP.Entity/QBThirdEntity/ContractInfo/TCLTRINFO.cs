using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.QBThirdEntity.ContractInfo
{
    /// <summary>
    /// 抵押物信息房产清单
    /// </summary>
    public class TCLTRINFO
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string NUMBER { get; set; }
        /// <summary>
        /// 房地产权编号
        /// </summary>
        public string FPR { get; set; }
        /// <summary>
        /// 房地产权地址
        /// </summary>
        public string ADDR { get; set; }
        /// <summary>
        /// 权属人
        /// </summary>
        public string OWNERSHIP { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public string FAREA { get; set; }
        /// <summary>
        /// 评估价值
        /// </summary>
        public string MONEY { get; set; }
        /// <summary>
        /// 租赁情况说明
        /// </summary>
        public string DES { get; set; }

    }
}
