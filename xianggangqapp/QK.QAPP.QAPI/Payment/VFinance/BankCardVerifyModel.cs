using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QK.QAPP.QAPI.Payment.VFinance
{
    /// <summary>
    /// 银行卡验证返回结果类
    /// </summary>
    public class VerifyResult
    {
        /// <summary>
        /// 验证返回状态
        /// </summary>
        public VerifyStatusEnum Status { get; set; }

        /// <summary>
        /// 返回Token
        /// </summary>
        public string VerityToken { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string ResultMessage { get; set; }
    }

    /// <summary>
    /// 银行卡验证参数类
    /// </summary>
    public class CkeckParam
    {
        /// <summary>
        /// 账户名称(姓名)
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 银行编码
        /// </summary>
        public string BankID { get; set; }

        /// <summary>
        /// 账户号码(银行卡号)
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// 卡类型(借记卡=DR，贷记卡=CR)
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 证件类型(目前只支持身份证ID_CARD)
        /// </summary>
        public string IdentificationType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 申请单号
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 验证渠道(民生渠道=CMBC,中金渠道=CPCN)
        /// </summary>
        public string VerifyChannel { get; set; }
    }

    /// <summary>
    /// 验证返回状态枚举
    /// </summary>
    public enum VerifyStatusEnum
    {
        /// <summary>
        /// 参数错误
        /// </summary>
        ParamError=0,

        /// <summary>
        /// 成功
        /// </summary>
        Sucess=1,

        /// <summary>
        /// 失败
        /// </summary>
        Failure=2,

        /// <summary>
        /// 处理中
        /// </summary>
        Process=3
    }
}