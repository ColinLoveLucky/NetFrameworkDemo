using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Services
{
    public class BID_PactHistoryService : IBID_PactHistoryService
    {
        /// <summary>
        /// 协议确认历史记录
        /// </summary>
        /// <param name="QueryPara"></param>
        /// <returns></returns>
       public PageData<QB_CONTRACT_HISTORY> AgreementHistoryList(BidListQueryPara QueryPara)
       {
           Dictionary<string, string> dic = new Dictionary<string, string>();
           NameValueCollection collection = new NameValueCollection();
           RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKAgreementHistoryList);
           collection = Serializer.ObjToNameValueCollection(QueryPara);
           dic = Serializer.ObjToDictionary(QueryPara);
           var AgreementHistoryData = restApiHelper.Post<PageData<QB_CONTRACT_HISTORY>>(restApiHelper.GetUrlParam(SecuritySignHelper.PostSecurityCollectionWithSign(collection)), dic);
           if (AgreementHistoryData == null)
           {
               return new PageData<QB_CONTRACT_HISTORY>();
           }
           return AgreementHistoryData;
       }
        /// <summary>
        /// 协议历史添加
        /// </summary>
        /// <param name="request"></param>
        /// <param name="contractOpType"></param>
        /// <returns></returns>
       public List<BidMatchTip> AgreementAddHistory(ContractHistoryRequest request)
       {
           RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKAgreementAddHistory);
           var operateData = restApiHelper.Post<List<BidMatchTip>>(string.Empty, request);
           return operateData;
       }
    }
}
