using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 试算请求参数
    /// </summary>
    public class CalcRequest
    {
        /// <summary>
        /// API版本，默认1.0
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// Qapp-申请系统
        /// </summary>
        public string sysid { get; set; }
        /// <summary>
        /// 借款客户证件号码
        /// </summary>
        public string idnum { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string pactno { get; set; }
        /// <summary>
        /// 签约日期,YYYYMMDD
        /// </summary>
        public string signdate { get; set; }
        /// <summary>
        /// 结束日期 ,YYYYMMDD 
        /// 等于 签约日期+期数
        /// </summary>
        public string enddate { get; set; }
        /// <summary>
        /// 期限月
        /// </summary>
        public string month { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        public string pactamt { get; set; }
        /// <summary>
        /// 年化利率，比如14.3
        /// </summary>
        public string rate { get; set; }
        /// <summary>
        /// 借款服务费率，比如1.5
        /// </summary>
        public string srate { get; set; }
        /// <summary>
        /// 是否固定还款日，0-否（日对日），1-是(固定还款日)
        /// </summary>
        public string isfixdate { get; set; }
        /// <summary>
        /// 借款类型，1-普通2-循环贷3-借新还旧
        /// </summary>
        public string occtype { get; set; }
        /// <summary>
        /// 当借款类型为 2、3时必为原合同号，当借款类型为1时传固定值”newPactno”
        /// </summary>
        public string oldPactno { get; set; }
        /// <summary>
        /// 还款方式1-等额本息3-利随本清 4-按月结息
        /// </summary>
        public string returntype { get; set; }
        /// <summary>
        /// 借款产品编号，由数据库统一同步到各系统或做转换
        /// </summary>
        public string kindno { get; set; }

    }
}
