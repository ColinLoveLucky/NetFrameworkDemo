/***************************************************************************
 * 描述：展期信息 
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
    public class RLOV_INFO
    {
        private string _CONT_SIGN_DATE = DateTime.Now.ToString("yyyy-MM-dd");
        /// <summary>
        /// 展期合同编号
        /// </summary>
        public string CONT_CODE { get; set; }
        /// <summary>
        /// 展期合同签署日期--
        /// 默认当天
        /// </summary>
        public string CONT_SIGN_DATE
        {
            get { return _CONT_SIGN_DATE; }
            set { this._CONT_SIGN_DATE = value; }
        }
        /// <summary>
        /// 展期 还款 开始日期
        /// </summary>
        public string PMT_STAT_DATE { get; set; }
        /// <summary>
        /// 展期还款结束日期
        /// </summary>
        public string PMT_END_DATE { get; set; }
        /// <summary>
        /// 展期签署地点
        /// </summary>
        public string CONT_SIGN_PLCE
        {
            get;
            set;
        }
        /// <summary>
        /// 展期资金渠道
        /// </summary>
        public string FUND_CHAN
        {
            get;
            set;
        }
        /// <summary>
        /// 展期签署地点省
        /// </summary>
        public string PROV { get; set; }
        /// <summary>
        /// 展期签署地点市
        /// </summary>
        public string CITY { get; set; }
        /// <summary>
        /// 展期签署地点区域代码
        /// </summary>
        public string REGN { get; set; }
        /// <summary>
        /// 展期签署地点不包括省市
        /// </summary>
        public string ADDR { get; set; }
        /// <summary>
        /// 展期咨询费
        /// </summary>
        public string RLOV_CNST_FEE { get; set; }

        /// <summary>
        /// 展期贷款本金数额
        /// </summary>
        public string RLOV_PRIC_AMT { get; set; }
        /// <summary>
        /// 展期贷款利息数额
        /// </summary>
        public string RLOV_RATE_AMT { get; set; }
        /// <summary>
        /// 展期期限
        /// </summary>
        public string RLOV_TERM { get; set; }
        /// <summary>
        /// 展期利率
        /// </summary>
        public string RLOV_RATE { get; set; }
        /// <summary>
        /// 展期月利率
        /// </summary>
        public string MONTH_RATE { get; set; }
        /// <summary>
        /// 展期还款日
        /// </summary>
        public string RLOV_PMT_DUE_DATE { get; set; }
        /// <summary>
        /// 上期贷款利息
        /// </summary>
        public string PREV_RATE_AMT { get; set; }
        /// <summary>
        /// 上期与本期贷款本金的差额
        /// </summary>
        public string PRIC_DIFF_AMT { get; set; }
        /// <summary>
        /// 展期借款服务费总额
        /// </summary>
        public string TOTL_SERV_FEE { get; set; }
        /// <summary>
        /// 展期逾期风险补偿
        /// </summary>
        public string RISK_COMP { get; set; }
        /// <summary>
        /// 展期借款月服务费
        /// </summary>
        public string SERV_FEE { get; set; }
        /// <summary>
        /// 展期利息
        /// </summary>
        public string RATE_AMT { get; set; }
        /// <summary>
        /// 展期贷款本息
        /// </summary>
        public string PRIC_RATE { get; set; }
        /// <summary>
        /// 展期借款人需要偿还金额
        /// </summary>
        public string NEED_AMT { get; set; }
        /// <summary>
        /// 展期起息日
        /// </summary>
        public string BEGN_INTR_DATE { get; set; }
    }
}
