using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class ExtendLoanPara
    {
        /// <summary>
        /// 原申请对象
        /// </summary>
        public APP_MAIN AppMainOld { get; set; }
        /// <summary>
        /// 新申请号中的展期次数
        /// </summary>
        public string OpeType { get; set; }
        /// <summary>
        /// 表单信息
        /// </summary>
        public Dictionary<string, string> FormDic { get; set; }
        /// <summary>
        /// 续展期数
        /// </summary>
        public int? PeriodAmt { get; set; }
        /// <summary>
        /// 已展期数
        /// </summary>
        public int? PeriodAmtUsed { get; set; }
        /// <summary>
        /// 展期或循环贷
        /// </summary>
        public string ActionGroup { get; set; }

        /// <summary>
        /// 已展期次数
        /// </summary>
        public int? TimesNumUsed { get; set; }
    }
}
