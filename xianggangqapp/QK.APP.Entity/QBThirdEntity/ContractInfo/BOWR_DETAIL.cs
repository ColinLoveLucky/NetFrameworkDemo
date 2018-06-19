/***************************************************************************
 * 描述：借款交易信息
 * 创建/修改时间：20160310
 * 创建/修改人：net team
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class BOWR_DETAIL
    {
        /// <summary>
        /// 还款日期
        /// </summary>
        public string PMT_DATE { get; set; }
        /// <summary>
        /// 还款开始日期
        /// </summary>
        public string STAT_DATE { get; set; }
        /// <summary>
        /// 还款结束日期
        /// </summary>
        public string END_DATE { get; set; }
        /// <summary>
        /// 起息日期
        /// </summary>
        public string BEGN_INTR_DATE { get; set; }
        /// <summary>
        /// 借款(贷款)本金
        /// </summary>
        public string PRIC_AMT { get; set; }
        /// <summary>
        /// 借款(贷款)利息
        /// </summary>
        public string RATE_AMT { get; set; }
        /// <summary>
        /// 第一期应还本息数额
        /// </summary>
        public string FIRT_PMT_AMT { get; set; }
        /// <summary>
        /// 第一期应还本息数额(含借款服务费)
        /// </summary>
        public string FIRT_SF_PMT_AMT { get; set; }
        /// <summary>
        /// 已偿还本金
        /// </summary>
        public string RAPD_AMT { get; set; }
        /// <summary>
        /// 剩余本金
        /// </summary>
        public string RESI_PRIC { get; set; }
        /// <summary>
        /// 逾期风险补偿金
        /// </summary>
        public string RISK_COMP { get; set; }
        /// <summary>
        /// 借款服务费总额
        /// </summary>
        public string TOTL_SERV_FEE { get; set; }
        /// <summary>
        /// 每期借款服务费
        /// </summary>
        public string SERV_FEE { get; set; }
        /// <summary>
        /// 借款咨询费
        /// </summary>
        public string CNST_FEE { get; set; }
        /// <summary>
        /// 借款咨询费(不包含风险补偿金)
        /// </summary>
        public string CNST_NRS_FEE { get; set; }
        /// <summary>
        /// 月偿还本息数额
        /// </summary>
        public string PMT_AMT_MOTH { get; set; }
         
         /// <summary>
        /// 剩余期数月偿还本息数额(含借款服务费)
        /// </summary>
        public string PMT_SF_AMT_MOTH { get; set; }
        /// <summary>
        /// 还款分期期数
        /// </summary>
        public string PMT_TERM { get; set; }
        /// <summary>
        /// 借款服务费分期
        /// </summary>
        public string TERM_MAX { get; set; }
        /// <summary>
        /// 申请金额
        /// </summary>
        public string REM_AMT { get; set; }

    }
}
