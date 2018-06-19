/***************************************************************************
 * 描述：产品信息 
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
    public class PROD_INFO
    {

        /// <summary>
        /// 贷款用途
        /// </summary>
        public string LOAN_PURPOSE { get;set; }
        /// <summary>
        /// 起始期数
        /// </summary>
        public string TERM_MIN { get; set; }
        /// <summary>
        /// 最大期数
        /// </summary>
        public string TERM_MAX { get; set; }
        /// <summary>
        /// 年利率
        /// </summary>
        public string RATE { get; set; }
        /// <summary>
        /// 还款方式
        /// </summary>
        public string RPMT_TYPE { get; set; }
        /// <summary>
        /// 年利率
        /// </summary>
       public string YEAR_RATE{get;set;}
        /// <summary>
        /// 月利率
        /// </summary>
       public string MONTH_RATE { get; set; }
        /// <summary>
        /// 利率类型
        /// </summary>
       public string RATE_TYPE { get; set; }
        /// <summary>
        /// 罚息利率
        /// </summary>
       public string PU_RATE { get; set; }
        /// <summary>
        /// GPS安装费
        /// </summary>
       public string INST_FEE_GPS { get; set; }
        /// <summary>
        /// GPS使用费
        /// </summary>
       public string SERV_FEE_GPS { get; set; }

    }
}
