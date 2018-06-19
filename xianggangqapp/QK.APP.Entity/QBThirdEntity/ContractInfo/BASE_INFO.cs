/***************************************************************************
 * 描述：基本信息 
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
    public class BASE_INFO
    {
        private string _CONT_SIGN_DATE = DateTime.Now.ToString("yyyy-MM-dd");
        /// <summary>
        /// 合同编号
        /// </summary>
        public string CONT_CODE { get; set; }
        /// <summary>
        /// 合同签署日期--
        /// 默认当天
        /// </summary>
        public string CONT_SIGN_DATE
        {
            get { return _CONT_SIGN_DATE; }
            set { this._CONT_SIGN_DATE = value; }
        }
        /// <summary>
        /// 签署地点
        /// </summary>
        public string CONT_SIGN_PLCE
        {
            get;
            set;
        }
        /// <summary>
        /// 资金渠道
        /// </summary>
        public string FUND_CHAN
        {
            get;
            set;
        }
        /// <summary>
        /// 签署地点省
        /// </summary>
        public string PROV { get; set; }
        /// <summary>
        /// 签署地点市
        /// </summary>
        public string CITY { get; set; }
        /// <summary>
        /// 签署地点区域代码
        /// </summary>
        public string REGN { get; set; }
        /// <summary>
        /// 签署地点不包括省市
        /// </summary>
        public string ADDR { get; set; }
        /// <summary>
        /// 展期还款日 原贷款合同-房屋抵押系统
        ///  内结清日往后推1 个月
        /// </summary>
        public string ONE { get; set; }
        /// <summary>
        /// 展期还款日 原贷款合同-房屋抵押系统
        ///  内结清日往后推2 个月
        /// </summary>
        public string TWO { get; set; }
        /// <summary>
        /// 展期还款日 原贷款合同-房屋抵押系统
        ///  内结清日往后推3个月
        /// </summary>
        public string THREE { get; set; }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public string HMT_STAT_DATE { get; set; }
        /// <summary>
        /// 合同结束日期
        /// </summary>
        public string HMT_END_DATE { get; set; }
        /// <summary>
        /// 公司客服电话
        /// </summary>
        public string CUS_SER_TEL { get; set; }
    }
}
