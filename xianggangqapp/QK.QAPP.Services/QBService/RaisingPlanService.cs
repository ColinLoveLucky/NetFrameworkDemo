/*************************************************************
 * 描述：募集计划Services
 * 时间：20160201
 * 操作人：leiz
 *************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.IServices;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.Global;
using System.Collections.Specialized;

namespace QK.QAPP.Services.QBService
{
    public class RaisingPlanService : IRaisingPlanService
    {
        public PageData<QB_RAISEPLAN> GetRaisingPlanList(ListViewBase para)
        {
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(para));
            var rest = new RestApiHelper(GlobalApi.GetRaisePlanList);
            var result = rest.Post<PageData<QB_RAISEPLAN>>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(para));
            if (result == null) { return new PageData<QB_RAISEPLAN>(); }
            return result;
        }

        public QB_RAISEPLAN GetRaisingPlanById(string id)
        {
            NameValueCollection collection = new NameValueCollection { { "id", id } };
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var rest = new RestApiHelper(GlobalApi.GetRaisePlanById);
            var result = rest.Get<QB_RAISEPLAN>(string.Empty, paras);
            if (result == null) { return new QB_RAISEPLAN(); }
            return result;
        }

        public PageData<QB_RAISEPLAN_HISTORY> GetRaisePlanHistory(RaisePlanListPara para)
        {
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(para));
            var rest = new RestApiHelper(GlobalApi.GetRaisePlanHistory);
            var result = rest.Post<PageData<QB_RAISEPLAN_HISTORY>>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(para));
            if (result == null) { return new PageData<QB_RAISEPLAN_HISTORY>(); }
            return result;
        }
    }
}
