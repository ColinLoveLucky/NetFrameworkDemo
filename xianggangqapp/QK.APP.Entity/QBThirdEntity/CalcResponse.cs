using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 试算返回参数
    /// </summary>
    public class CalcResponse
    {
        /// <summary>
        /// 0000	成功访问接口（其他都是失败），当为0000时，content内容为正常还款计划json数据(其它content为空)
        /// 1001	参数传递错误
        /// 1002	版本不支持
        /// 1003	参数格式错误
        /// 1004	产品已失效等业务错误
        /// 1005	系统内部错误
        /// </summary>
        public string code { get; set; }
        public string message { get; set; }
        public List<Content> content { get; set; }
        public class Content
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string serialno { get; set; }
            /// <summary>
            /// 借款客户证件号码
            /// </summary>
            public string idnum { get; set; }
            /// <summary>
            /// 合同编号
            /// </summary>
            public string pactno { get; set; }
            /// <summary>
            /// 合同金额
            /// </summary>
            public decimal pactamt { get; set; }
            /// <summary>
            /// 本期本金
            /// </summary>
            public decimal principle { get; set; }
            /// <summary>
            /// 本期利息
            /// </summary>
            public decimal interest { get; set; }
            /// <summary>
            /// 本期本息decimal(16,2),不包含本期借款服务费
            /// </summary>
            public decimal returnsum { get; set; }
            /// <summary>
            /// 本期借款服务费
            /// </summary>
            public decimal bromanfee { get; set; }
            /// <summary>
            /// 期号
            /// </summary>
            public int termnum { get; set; }
            /// <summary>
            /// 本期开始日期
            /// </summary>
            public string opndate { get; set; }
            /// <summary>
            /// 本期应还日期
            /// </summary>
            public string enddate { get; set; }
            /// <summary>
            /// 还款方式1-等额本息3-利随本清 4-按月结息
            /// </summary>
            public string returntype { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string filler { get; set; }

        }

    }
}
