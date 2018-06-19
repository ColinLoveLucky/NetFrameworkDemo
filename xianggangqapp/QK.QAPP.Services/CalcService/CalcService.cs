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
    public class CalcService : ICalcService
    {
        /// <summary>
        /// 调用试算接口
        /// </summary>
        /// <param name="calcRequest"></param>
        /// <param name="calcurl">接口地址</param>
        /// <returns></returns>
        public CalcResponse GetCalcResult(CalcRequest calcRequest,string calcurl)
        {
            var json = Serializer.ToJson(calcRequest);
            var calcUrl = calcurl;
            var rest = new RestApiHelper(calcUrl);
            //post 
           // var postResult = rest.Post<CalcResponse>(string.Empty, calcRequest);
            //var postResult1 = rest.Post<CalcResponse>(string.Empty, Serializer.ObjToDictionary(calcRequest));

            //http://xxx/api/plan/version/sysid/idnum/pactno/signdate/enddate/month/pactamt/rate/srate/isfixdate/occtype/oldPactno/returntype/kindno



            //get
            //string testParas = "1.0/Qapp/440104196404065311/YWPZ02201603110402630/20160304/20160604/3/20000.00/14.3/1.5/0/1/newPactno/1/5023";
            string paras = string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}/{14}", calcRequest.version, calcRequest.sysid, calcRequest.idnum, calcRequest.pactno, calcRequest.signdate, calcRequest.enddate, calcRequest.month, calcRequest.pactamt, calcRequest.rate, calcRequest.srate, calcRequest.isfixdate, calcRequest.occtype,calcRequest.oldPactno, calcRequest.returntype, calcRequest.kindno);
            var getResult = rest.Get<CalcResponse>(paras, null);

            if (getResult == null) { return new CalcResponse(); }

            return getResult;
        }
        /// <summary>
        /// 回写固定还款日到标的系统
        /// </summary>
        /// <param name="request"></param>
        /// <param name="QKSetRepayMentDay">接口地址</param>
        /// <returns></returns>
        public BidMatchTip SetRepayMentDay(PreCalcRequest request, string QKSetRepayMentDay)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            RestApiHelper restApiHelper = new RestApiHelper(QKSetRepayMentDay);
            dic = Serializer.ObjToDictionary(request);
            var operateData = restApiHelper.Post<BidMatchTip>(string.Empty, dic);
            return operateData;
        }
        /// <summary>
        /// 回写签约时间
        /// </summary>
        /// <param name="request"></param>
        /// <param name="QKSetBidSignedTime"></param>
        /// <returns></returns>
        public BidMatchTip SetBidSignedTime(BidAgreementRequest request, string QKSetBidSignedTime)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            RestApiHelper restApiHelper = new RestApiHelper(QKSetBidSignedTime);
            dic = Serializer.ObjToDictionary(request);
            var operateData = restApiHelper.Post<BidMatchTip>(string.Empty, dic);
            return operateData;
        }
    }
}
