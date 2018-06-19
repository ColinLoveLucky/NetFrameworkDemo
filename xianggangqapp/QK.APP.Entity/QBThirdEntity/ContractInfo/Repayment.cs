/***************************************************************************
 * 描述：还款计划
 * 创建/修改时间：20160318
 * 创建/修改人：net team
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.QBThirdEntity.ContractInfo
{
    /// <summary>
    /// 还款计划
    /// </summary>
    public class REPAYMENT
    {
        /// <summary>
        /// 期数
        /// </summary>
        public string QS { get; set; }
        /// <summary>
        /// 还款日期
        /// </summary>
        public string PMT_DATE { get; set; }
        /// <summary>
        /// 月还款额=本金+利息+服务费
        /// </summary>
        public string PMT_AMT_MOTH { get; set; }
        /// <summary>
        /// 剩余本金
        /// </summary>
        public string RESI_PRIC { get; set; }
        /// <summary>
        /// 偿还本息（元）
        /// </summary>
        public string T_PMT_AMT{get;set;}
        /// <summary>
        /// 偿还本金（元）
        /// </summary>
        public string T_PRIC_AMT{get;set;}
        /// <summary>
        /// 偿还利息（元）
        /// </summary>
        public string T_RATE_AMT{get;set;}
        /// <summary>
        /// 借款服务费（元）
        /// </summary>
        public string  T_SERV_FEE{get;set;}
        /// <summary>
        /// 一次性还款金额
        /// </summary>
        public string T_LS_REPAY_AMO { get; set; }
        					
    }
}
