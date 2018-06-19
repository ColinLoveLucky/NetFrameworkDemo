using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using QK.QAPP.Infrastructure;
using QK.QAPP.Global;
using Newtonsoft.Json;
using QK.QAPP.QAPI.Payment.VFinance;
using QK.QAPP.QAPI.App_Code;

namespace QK.API.UnitTest
{
    [TestClass]
    public class CFCA_PaymentTest
    {
        [TestMethod]
        public void BankVerifyCheckTest()
        {
            var paras = new Dictionary<string, string>
                        {
                            {"AccountName", "吴晓峰"},
                            {"BankID", "100"},
                            {"AccountNumber", "223903343553434434"},
                            {"CardType","DR"},
                            {"AppId",GlobalSetting.BankCardVerifyAppID},
                            {"IdentificationType", "ID_CARD"},
                            {"IdentificationNumber", "310112198309080054"},
                            {"Mobile", "13656788322"},
                            {"AppCode", "10239"},
                            {"VerifyChannel", "CPCN"}
                        };
            var restClient = new RestHelper("http://localhost:50145/api/CFCA_Payment");//GlobalSetting.CFCAPaymentURL
            var returnResult = restClient.Get<String>("", paras);  //BankCardVerifyCheck
            if (returnResult != null && returnResult.Status == DtoMessageStatus.Success)
            {
                var returnObject = JsonConvert.DeserializeObject<VerifyResult>(returnResult.ReturnObj);
                Assert.IsTrue(returnObject.Status == VerifyStatusEnum.Sucess,returnObject.VerityToken);
            }
        }

        [TestMethod]
        public void BankVerifyQueryTest()
        {
            var paras = new Dictionary<string, string>
                        {
                            {"VerifyToken", "8c692f3ad0f64ff19a4cc7aa598b286e"},
                            {"AppCode", "10239"}
                        };
            var restClient = new RestHelper("http://localhost:50145/api/CFCA_Payment");//GlobalSetting.CFCAPaymentURL
            var returnResult = restClient.Get<String>("", paras); //BankCardVerifyQuery
            if (returnResult != null && returnResult.Status == DtoMessageStatus.Success)
            {
                var returnObject = JsonConvert.DeserializeObject<VerifyResult>(returnResult.ReturnObj);
                Assert.IsTrue(returnObject.Status == VerifyStatusEnum.Sucess, returnObject.VerityToken);
            }
        }
    }
}
