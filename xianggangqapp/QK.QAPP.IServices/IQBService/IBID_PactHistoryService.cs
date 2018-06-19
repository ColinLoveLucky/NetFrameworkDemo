using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
   public interface IBID_PactHistoryService
    {
       /// <summary>
       /// 协议确认历史列表
       /// </summary>
       /// <param name="QueryPara"></param>
       /// <returns></returns>
        PageData<QB_CONTRACT_HISTORY> AgreementHistoryList(BidListQueryPara QueryPara);
       /// <summary>
       /// 协议确认历史添加
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
         List<BidMatchTip> AgreementAddHistory(ContractHistoryRequest request);
    }
}
