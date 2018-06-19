using Newtonsoft.Json;
using QK.QAPP.Entity;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Infrastructure;
using QK.QAPP.Entity.ExtendEntity;


namespace QK.QAPP.Services.QAPPApiService
{
    public class SyncBidProductService : ISyncBidProductService
    {
        public IAPP_MAINSERVICE AppMainService { get; set; }
        public IAPP_LOANSERVICE AppLoanService { get; set; }
        public IAPP_QUEUESERVICE AppQueueService { get; set; }
        public IQFProductInfoService QFProductInfoService { get; set; }
        public IAPP_CUSTOMERSERVICE AppCustomerService { get; set; }

        #region 根据申请代码,更新AppMain信息表
        /// <summary>
        /// 根据申请代码,更新AppMain信息表
        /// 2016年5月9日16:32:35  by shawn
        /// </summary>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        public APP_MAIN UpdateMainByProductCode(Dictionary<string, string> formDic, QFProductInfo product, out string resultMsg)
        {
            resultMsg = string.Empty;
            var appCode = formDic["APP_CODE"];
            var appMain = AppMainService.FirstOrDefault(m => m.APP_CODE == appCode);
            if (appMain == null)
            {
                resultMsg = string.Format("APP_Code:{0},在APP_MAIN 表无此记录。", appCode);
                return null;
            }

            var appQueue = AppQueueService.Find(q => q.APP_CODE == appCode).FirstOrDefault();
            if (appQueue == null)
            {
                resultMsg = string.Format("APP_Code:{0},在APP_QUEUE 表无此记录。", appCode);
                return null;
            }

            var appLoan = AppLoanService.Find(loan => loan.APP_ID == appMain.ID).FirstOrDefault();
            if (appLoan == null)
            {
                resultMsg = string.Format("APP_Code:{0},在AppLoan 表无此记录。", appCode);
                return null;
            }

            var appCustomer = AppCustomerService.Find(user => user.APP_ID == appMain.ID).FirstOrDefault();
            if (appCustomer == null)
            {
                resultMsg = string.Format("APP_ID:{0},在AppCustomer 表无此记录。", appMain.ID);
                return null;
            }

            if (formDic.Keys.Contains("LOGO") && !string.IsNullOrEmpty(formDic["LOGO"]))
                appMain.LOGO = formDic["LOGO"];

            if (formDic.Keys.Contains("PRODUCT_CODE") && !string.IsNullOrEmpty(formDic["PRODUCT_CODE"]))
                appMain.PRODUCT_CODE = formDic["PRODUCT_CODE"];

            if (formDic.Keys.Contains("PRODUCT_NAME") && !string.IsNullOrEmpty(formDic["PRODUCT_NAME"]))
                appMain.PRODUCT_NAME = formDic["PRODUCT_NAME"];

            if (formDic.Keys.Contains("PROD_VERSION") && !string.IsNullOrEmpty(formDic["PROD_VERSION"]))
                appMain.PROD_VERSION = formDic["PROD_VERSION"];


            var srate = ((product.pProduct.consultationChargeRatio - product.pProduct.reservesRatio) * 100).ToString();

            appQueue.TERMS = GetProductTerns(formDic["APP_CODE"]);     // 计算期数
            appLoan.LOAN_AMT_OF_CONTRACT = AccurateHundredsDigit(appLoan.LOAN_AMT / (1 - product.pProduct.consultationChargeRatio));   // 合同金额(到手金额(loan_amt) /（1 – 前期咨询费费率）【计算结果保留百位去整】)  
            appLoan.PAY_AMT_FIRST_MONTHLY = GetPayAmountFirstMonth(formDic["ContractNo"], formDic["PRODUCT_CODE"], product.pLogo.logo,
                formDic["ContractNo"], product.pInterest.rate.ToString(), srate, appCustomer.ID_NO);  //  首月还款额 调用第三方试算接口
            appLoan.PAY_AMT_LAST_MONTHLY = GetPayAmountLastMonth(formDic["ContractNo"], formDic["PRODUCT_CODE"], product.pLogo.logo,
                formDic["ContractNo"], product.pInterest.rate.ToString(), srate, appCustomer.ID_NO);   //  末月还款额 调用第三方试算接口
            appLoan.PAY_AMT_MONTHLY = appLoan.PAY_AMT_LAST_MONTHLY;        // 月还款 调用第三方试算接口
            appLoan.DEFAULT_INTEREST_RATIO = product.pInterest.defaultInterestRatio; // 罚息比率
            appLoan.LATE_FEE_RATIO = product.pInterest.lateFeeRatio;         // 滞纳金比率
            appLoan.CONSULTATION_CHARGE_AMT = appLoan.LOAN_AMT_OF_CONTRACT * (product.pProduct.consultationChargeRatio - product.pProduct.reservesRatio);  //  咨询费 不含准备金 (合同金额  *（前期咨询费率 - 风险准备金率）)
            appLoan.CONSULTATION_CHARGE_RATIO = product.pProduct.consultationChargeRatio; // 咨询费率 (含准备金率)
            appLoan.SERVICE_CHARGE_AMT = appLoan.LOAN_AMT_OF_CONTRACT * product.pInterest.serviceChargeRatio;       // 月服务费
            appLoan.SERVICE_CHARGE_RATIO = product.pInterest.serviceChargeRatio;   //  服务费率
            appLoan.RATE = product.pInterest.rate;                   // 年利率
            appLoan.RATE_TYPE = product.pInterest.rateType;             // 利率类型
            appLoan.LATE_FEE_RATIO = product.pInterest.lateFeeRatio;         // 提前还款手续费比例
            appLoan.TERMS = GetProductTerns(formDic["APP_CODE"]);                  // 期数
            appLoan.RESERVES_RATIO = product.pProduct.reservesRatio;         // 风险准备金率
            appLoan.RESERVES_AMT = appLoan.LOAN_AMT_OF_CONTRACT * product.pProduct.reservesRatio;             // 风险准备金 (合同金额 * 风险准备金率)

            AppMainService.Update(appMain);
            AppQueueService.Update(appQueue);
            AppLoanService.Update(appLoan);

            AppMainService.UnitOfWork.SaveChanges();

            return appMain;
        }
        #endregion

        #region 计算首月还款额
        /// <summary>
        /// 计算首月还款额
        /// </summary>
        /// <param name="pactNo">合同编号</param>
        /// <param name="productCode">产品编号</param>
        /// <param name="pLogo">借款产品编号</param>
        /// <param name="pactamt">合同金额</param>
        /// <param name="rate">年化利率</param>
        /// <param name="srate">借款服务费率</param>
        /// <param name="idNum">借款客户证件号码</param>
        /// <returns></returns>
        private decimal? GetPayAmountFirstMonth(string pactNo, string productCode, string pLogo, string pactamt, string rate, string srate, string idNum)
        {
            var contract = GetContractResult(pactNo, productCode, pLogo, pactamt, rate, srate, idNum).Content;
            if (contract == null)
                return 0;

            return CaculatePayAmount(contract.FirstOrDefault()).ToDecimal();
        }
        #endregion

        #region 末月还款额/月还款
        /// <summary>
        /// 末月还款额/月还款
        /// </summary>
        /// <param name="pactNo">合同编号</param>
        /// <param name="productCode">产品编号</param>
        /// <param name="pLogo">借款产品编号</param>
        /// <param name="pactamt">合同金额</param>
        /// <param name="rate">年化利率</param>
        /// <param name="srate">借款服务费率</param>
        /// <param name="idNum">借款客户证件号码</param>
        /// <returns></returns>
        private decimal? GetPayAmountLastMonth(string pactNo, string productCode, string pLogo, string pactamt, string rate, string srate, string idNum)
        {
            var contract = GetContractResult(pactNo, productCode, pLogo, pactamt, rate, srate, idNum).Content;

            if (contract == null)
                return 0;

            return CaculatePayAmount(contract.FirstOrDefault()).ToDecimal();
        }
        #endregion

        #region 计算还款额度
        /// <summary>
        /// 计算还款额度
        /// 计算结果：本期本金+本期利息+本期借款服务费
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        private double CaculatePayAmount(Contract contract)
        {
            return contract.Principle + contract.Interest + contract.Bromanfee;
        }
        #endregion

        #region 获取产品的期数
        /// <summary>
        /// 获取产品的期数
        /// 2016年5月9日18:00:13 by shawn 
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        private short? GetProductTerns(string productCode)
        {
            var terns = QFProductInfoService.GetQFProductTerm(productCode);
            if (terns.Count == 0)
                return 0;
            return (short?)terns[0];
        }
        #endregion

        #region 获取贷款产品的信息
        private ContractResult GetContractResult(string pactNo, string productCode, string pLogo, string pactamt, string rate, string srate, string idNum)
        {
            var request = new ContractRequest
            {
                Enddate = CaculateEndDatePayAmount(productCode),
                Idnum = idNum,
                Isfixdate = "1",  // 固定还款日
                Kindno = pLogo,
                Month = GetProductTerns(productCode).ToString(),
                Occtype = "1",
                OldPactno = "newPactno",
                Pactamt = pactamt,
                Pactno = pactNo,
                Rate = rate,
                Returntype = "2",
                Signdate = DateTime.Now.ToLongDateString().ToString(),
                Srate = srate,
                Sysid = "Qapp",
                Version = "1.0"
            };

            var response = ContarctRequest(request);

            return JsonConvert.DeserializeObject<ContractResult>(response);
        }

        #region 计算还款结束日期
        /// <summary>
        /// 计算还款结束日期:当天加上期数（2016年5月10日10:04:15+12期）
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        private string CaculateEndDatePayAmount(string productCode)
        {
            return DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"))
                        .AddMonths(GetProductTerns(productCode).ToInt32())
                        .AddDays(-1)
                        .ToShortDateString();
        }
        #endregion

        private static string ContarctRequest(object request)
        {
            var json = JsonConvert.SerializeObject(request);

            // 记录请求报文
            Trace.Write(string.Format("{0}{1}", "请求报文:", json));

            var response = string.Empty;

            try
            {
                response = WebRequestHelper.Request(WebRequestHelper.Method.POST, Global.GlobalSetting.ContarctApiUrl, "application/json", json);
            }
            catch (Exception ex)
            {
                throw new Exception("异常:" + ex.Message);
            }

            // 记录响应的报文
            Trace.Write(string.Format("{0}{1}", "响应报文:", response));

            return response;
        }
        #endregion

        /// <summary>
        /// 计算结果百位取整
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private decimal? AccurateHundredsDigit(decimal? amount)
        {
            if (amount.HasValue)
            {
                amount = amount / 100;
                amount = Math.Round(amount.Value, MidpointRounding.AwayFromZero);
                return amount * 100;
            }
            else
            {
                return null;
            }
        }

        public void Test()
        {
            var list = GetContractResult("1", "productCodexyd-elite-5-24", "1", "1", "1", "1", "1");
            var temp = list;
        }
    }
}
