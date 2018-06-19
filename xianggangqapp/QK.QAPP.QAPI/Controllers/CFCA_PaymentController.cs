using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Configuration;
using CFCA.Payment.Api;
using System.Diagnostics;
using QK.API.Infrastructure;
using VFinance;
using Newtonsoft.Json;
using QK.QAPP.QAPI.Payment.VFinance;
using QK.QAPP.QAPI.App_Code;

namespace QK.QAPP.QAPI.Controllers
{
    public class CFCA_PaymentController : ApiController
    {
        /// <summary>
        /// 机构编号
        /// </summary>
        private string InstitutionID = WebConfigurationManager.AppSettings["InstitutionID"];
        /// <summary>
        /// 是否调试
        /// </summary>
        private bool IsDebug = WebConfigurationManager.AppSettings["IsDebug"] == "1" ? true : false;
        /// <summary>
        /// 账户类型：11个人账户，12企业账户
        /// </summary>
        private int iAccountType = Convert.ToInt32(WebConfigurationManager.AppSettings["AccountType"]);
        /// <summary>
        /// 中金提供的测试用银行编码
        /// </summary>
        private string DebugBankCode = WebConfigurationManager.AppSettings["DebugBankCode"];

        /// <summary>
        /// 银行卡验证接口
        /// 创建人:张浩
        /// 创建日期：2016-01-18
        /// </summary>
        /// <param name="param">param其各属性如下：
        /// AccountName:  日程开始时间
        /// BankID:  银行编码
        /// AccountNumber:  账户号码(银行卡号)
        /// CardType:  卡类型(借记卡=DR，贷记卡=CR)
        /// AppId:  应用ID
        /// IdentificationType:  证件类型(目前只支持身份证=ID_CARD)
        /// IdentificationNumber:  证件号码
        /// Mobile:  手机号
        /// AppCode:  申请单号
        /// VerifyChannel:  验证渠道(民生渠道=CMBC,中金渠道=CPCN)</param>
        /// <returns>银行卡验证结果</returns>
        [HttpGet]
        public VerifyResult BankCardVerifyCheck([FromUri] CkeckParam param)  
        {
            VerifyResult result = new VerifyResult();
            try
            {
                DefaultBankCardVerifyFacadeService facade = new DefaultBankCardVerifyFacadeService();
                #region 验证信息
                kvp[] verifyInfo = new kvp[]{
                    new kvp()
                    {
                        key="cert_type",
                        value=param.IdentificationType
                    },        
                    new kvp()
                    {
                        key="cert_no",
                        value=param.IdentificationNumber
                    },
                    new kvp()
                    {
                        key="name",
                        value=param.AccountName
                    },
                    new kvp()
                    {
                        key="mobile",
                        value=param.Mobile
                    }
                };
                #endregion
                bankCardCheckVerifyRequest checkParam = new bankCardCheckVerifyRequest()
                {
                    appId = param.AppId,
                    bankCardNo = param.AccountNumber,
                    bankCode =APIHelper.ConvertBankCode(param.BankID),  //转换银行编码
                    cardType = param.CardType,
                    verifyInfo = verifyInfo
                };
                if (!string.IsNullOrEmpty(param.VerifyChannel))  //验证渠道
                {
                    checkParam.verifyChannel = param.VerifyChannel;
                }
                //记录请求报文
                Trace.Write(param.AppCode + "_Check_Request$" + JsonConvert.SerializeObject(checkParam), "VFinance");
                var checkResult=facade.check(checkParam);  //调用申请验证接口
                //记录响应报文
                Trace.Write(param.AppCode + "_Check_Response$" + JsonConvert.SerializeObject(checkResult), "VFinance");
                result.VerityToken = checkResult.verifyToken; //记录返回的Token
                SetStatus(result, checkResult);
            }
            catch (Exception ex)
            {
                Trace.Write(param.AppCode + "_VFinance异常$" + ex.Message.ToString(), "VFinance");
                result.Status = VerifyStatusEnum.Failure;
            }
            return result;
        }

        /// <summary>
        /// 银行卡验证结果查询接口
        /// 创建人:张浩
        /// 创建日期：2016-01-18
        /// </summary>
        /// <param name="VerifyToken">验证Token</param>
        /// <param name="AppCode">申请单号</param>
        /// <returns>银行卡验证结果</returns>
        [HttpGet]
        public VerifyResult BankCardVerifyQuery(string VerifyToken, string AppCode)
        {
            VerifyResult result = new VerifyResult();
            try
            {
                DefaultBankCardVerifyFacadeService facade = new DefaultBankCardVerifyFacadeService();
                queryVerifyRequest queryParam = new queryVerifyRequest()
                {
                    verifyToken = VerifyToken
                };
                //记录请求报文
                Trace.Write(AppCode + "_Query_Request$" + JsonConvert.SerializeObject(queryParam), "VFinance");
                var queryResult = facade.query(queryParam);   //调用验证查询接口
                //记录响应报文
                Trace.Write(AppCode + "_Query_Response$" + JsonConvert.SerializeObject(queryResult), "VFinance");
                result.VerityToken = queryParam.verifyToken; //记录返回的Token
                SetStatus(result, queryResult);
            }
            catch (Exception ex)
            {
                Trace.Write(AppCode + "_VFinance异常$" + ex.Message.ToString(), "VFinance");
                result.Status = VerifyStatusEnum.Failure;
            }
            return result;
        }

        /// <summary>
        /// 设置返回的验证状态
        /// </summary>
        /// <param name="result">WebAPI的返回值</param>
        /// <param name="serviceResult">服务接口的返回值</param>
        private static void SetStatus(VerifyResult result, checkVerifyResult serviceResult)
        {
            if (serviceResult.success && serviceResult.verifyStatus == "S")  //渠道验证通过
            {
                result.Status = VerifyStatusEnum.Sucess;
            }
            else if (serviceResult.success && serviceResult.verifyStatus == "P")  //渠道验证中/认证申请已落地，渠道异常(需要调用query方法查询验证结果)
            {
                result.Status = VerifyStatusEnum.Process;
            }
            else
            {
                result.Status = serviceResult.success ? VerifyStatusEnum.Failure : VerifyStatusEnum.ParamError;
            }
            result.ErrorCode = serviceResult.errorCode;
            if (!string.IsNullOrEmpty(result.ErrorCode))
            {
                switch (result.ErrorCode)
                {
                    case "P001":
                        result.ResultMessage = "处理中";
                        break;
                    case "F001":
                        result.ResultMessage = "验证不通过";
                        break;
                    case"F101":
                        result.ResultMessage = "参数校验不通过";
                        break;
                    case "E001":
                        result.ResultMessage = "内部异常";
                        break;
                    case "E003":
                        result.ResultMessage = "校验频率过快";
                        break;
                    default:
                        result.ResultMessage = "未知错误码";
                        break;
                }
                //result.ResultMessage = serviceResult.resultMessage;
            }
        }
    }
}
