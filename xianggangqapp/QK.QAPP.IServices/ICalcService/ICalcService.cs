using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface ICalcService
    {
        /// <summary>
        /// 调用试算接口
        /// </summary>
        /// <param name="calcRequest"></param>
        /// <param name="calurl">接口地址</param>
        /// <returns></returns>
        CalcResponse GetCalcResult(CalcRequest calcRequest,string calurl);
        /// <summary>
        /// 回写固定还款日到标的系统
        /// </summary>
        /// <param name="request"></param>
        /// <param name="QKSetRepayMentDay">接口地址</param>
        /// <returns></returns>
        BidMatchTip SetRepayMentDay(PreCalcRequest request, string QKSetRepayMentDay);
        /// <summary>
        /// 回写签约时间
        /// </summary>
        /// <param name="request"></param>
        /// <param name="QKSetBidSignedTime"></param>
        /// <returns></returns>
        BidMatchTip SetBidSignedTime(BidAgreementRequest request, string QKSetBidSignedTime);
    }
}
