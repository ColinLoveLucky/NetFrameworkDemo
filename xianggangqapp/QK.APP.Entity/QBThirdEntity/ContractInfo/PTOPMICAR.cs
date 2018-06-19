/***************************************************************************
 * 描述：附件
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
    /// 出借人及还款信息
    /// </summary>
    public class PTOPMICAR
    {
        /// <summary>
        /// 债权编号
        /// </summary>
        public string DEBT_NO { get; set; }
        /// <summary>
        /// 出借人用户名
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        /// 出借人账户号
        /// </summary>
        public string BAND_DEPT_ACNT { get; set; }
        /// <summary>
        /// 本次出借金额
        /// </summary>
        public string MATC_AMT { get; set; }
        /// <summary>
        /// 出借金额占比
        /// </summary>
        public string MATC_PERC { get; set; }
        /// <summary>
        /// 还款起始日期
        /// </summary>
        public string PMT_STAT_DATE { get; set; }
        /// <summary>
        /// 还款期数（月）
        /// </summary>
        public string PMT_TERM { get; set; }
        /// <summary>
        /// 首期应还本息数额（元）
        /// </summary>
        public string FIRT_PMT_AMT { get; set; }

        /// <summary>
        /// 剩余期数应还本息数额（元）
        /// </summary>
        public string RESI_PRIC { get; set; }
    }
}
