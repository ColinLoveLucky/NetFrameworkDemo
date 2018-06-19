using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public class ContractRequest
    {
       /// <summary>
        /// API版本，默认1.0-小贷通 2.0-T24 (Default:1.0)
       /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Qapp-申请系统  (Default:Qapp)
        /// </summary>
        public string Sysid { get; set; }

        /// <summary>
        /// 借款客户证件号码，不区分大小写 1 (Id_No)
        /// </summary>
        public string Idnum { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string Pactno { get; set; }

        /// <summary>
        /// 签约日期 (取当天时间)
        /// </summary>
        public string Signdate { get; set; }

        /// <summary>
        /// 结束日期 (当天加上期数)
        /// </summary>
        public string Enddate { get; set; }

        /// <summary>
        /// 期限月 (期数)
        /// </summary>
        public string Month { get; set; }
        
        /// <summary>
        /// 合同金额 
        /// </summary>
        public string Pactamt { get; set; }

        /// <summary>
        /// 年化利率
        /// </summary>
        public string Rate { get; set; }

        /// <summary>
        /// 借款服务费率
        /// </summary>
        public string Srate { get; set; }

        /// <summary>
        /// 是否固定还款日，0-否（日对日），1-是(固定还款日)
        /// </summary>
        public string Isfixdate { get; set; }

        /// <summary>
        /// 借款类型，1-普通2-循环贷3-借新还旧
        /// </summary>
        public string Occtype { get; set; }

        /// <summary>
        /// 当借款类型为 2、3时必为原合同号，当借款类型为1时传固定值”newPactno”
        /// </summary>
        public string OldPactno { get; set; }

        /// <summary>
        /// 还款方式1-等额本息3-利随本清 4-按月结息
        /// </summary>
        public string Returntype { get; set; }

        /// <summary>
        /// 借款产品编号，由数据库统一同步到各系统或做转换
        /// </summary>
        public string Kindno { get; set; }
    }
}
