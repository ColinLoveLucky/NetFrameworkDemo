using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity.ExtendEntity;
using QK.QAPP.Entity.QbEntity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices.IQBService;

namespace QK.QAPP.Services.QBService
{
    public class BidSystemConfigService : IBidSystemConfigService
    {
        /// <summary>
        /// 获取标的系统配置信息
        /// </summary>
        /// <returns></returns>
        public List<BidSystemConfigInfo> GetBidSystemConfigList()
        {
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(null);
            var rest = new RestApiHelper(GlobalApi.QKSysConfigInfoList);
            var result = rest.Get<List<BidSystemConfigInfo>>(string.Empty, paras);
            if (result != null)
            {
                return result;
            }
            return new List<BidSystemConfigInfo>();
        }

        /// <summary>
        /// 通过ID获取标的系统配置
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public BidSystemConfigInfo GetInfoById(string Id)
        {
            var collection = new NameValueCollection
            {
                {"id", Id}
            };
            RestApiHelper rest = new RestApiHelper(GlobalApi.GetQKSysConfigById);
            var securityKey = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            return rest.Get<BidSystemConfigInfo>(string.Empty, securityKey);
        }


        public void Update(BidSystemConfigInfo model)
        {
            RestApiHelper rest = new RestApiHelper(GlobalApi.QKSysConfigUpdate);
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(model));
            rest.Post<BidSystemConfigInfo>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(model));
        }

        public bool IsExistKey(string key)
        {
            var collection = new NameValueCollection
            {
                {"key", key}
            };
            RestApiHelper rest = new RestApiHelper(GlobalApi.QKSysConfigIsExistKey);
            var securityKey = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            return rest.Get<bool>(string.Empty, securityKey);
        }

        public void Add(BidSystemConfigInfo model)
        {
            var input = Serializer.ObjToNameValueCollection(model);
            RestApiHelper rest = new RestApiHelper(GlobalApi.QKSysConfigAdd);
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(input);
            rest.Post<BidSystemConfigInfo>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(model));
        }


        public bool Delete(string Id)
        {
            var collection = new NameValueCollection
            {
                {"id", Id}
            };
            RestApiHelper rest = new RestApiHelper(GlobalApi.QKSysConfigDelete + string.Format("/{0}", Id));
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(collection);
            return rest.Post<bool>(rest.GetUrlParam(securityKey), new Dictionary<string, string>() { { "id", Id } });
        }
    }
}
