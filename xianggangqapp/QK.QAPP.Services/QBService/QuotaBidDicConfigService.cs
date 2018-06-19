using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System.Collections.Specialized;
using Microsoft.Practices.Unity;
using QK.QAPP.Infrastructure.Cache;

namespace QK.QAPP.Services
{
    public class QuotaBidDicConfigService : IQuotaBidDicConfigService
    {
        public ICacheProvider CacheService { get; set; }
        public IAPP_CITYSERVICE OrgService { get; set; }
        public List<QB_DICTIONARY> GetQbDicType(string dicType)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("dicType", dicType);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var rest = new RestApiHelper(GlobalApi.QKDictionary);
            var result = rest.Get<List<QB_DICTIONARY>>(string.Empty, paras);
            if (result != null)
            {
                return result;
            }
            return new List<QB_DICTIONARY>();

        }
        
        public Dictionary<string, string> GetQbDicByType(string dicType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var dicList = GetQbDicType(dicType);
            if (dicList != null)
            {
                //针对额度  特殊处理
                if (dicType == "QuotaType")
                {
                    var quotaDicList = GetQbDicType(dicType).Where(p => p.LEVEL_NO == 2).ToList();//额度 过滤掉大类型
                    var parentDic = dicList.Where(d => d.LEVEL_NO == 1).ToDictionary(k => k.DIC_CODE, v => v.DIC_NAME);
                    quotaDicList.ForEach(p =>
                    {
                        if (parentDic.ContainsKey(p.PARENT_CODE))
                        {
                            p.DIC_NAME = parentDic[p.PARENT_CODE] + "->" + p.DIC_NAME;
                        }

                    });
                    return quotaDicList.ToDictionary(k => k.DIC_CODE, v => v.DIC_NAME);
                }
                return dicList.ToDictionary(k => k.DIC_CODE, v => v.DIC_REMARK);
            }
            return dic;
        }
        public Dictionary<string, string> GetOrgList()
        {
            var org = OrgService.GetOrgRoleList().Where(g => g.COMPANYNAME == "QuarkFinance").ToDictionary(k => k.OBJECTID, v => v.OBJECTNAME);
            return org;
        }
        public Dictionary<string,string> GetRejectReason()
        {
            var keyData = CacheService.GetFromCacheOrProxy<Dictionary<string, string>>(
              "QB_GetRejectReason",
              () => GetQbDicType("BidRejectReason").ToDictionary(k=>k.DIC_CODE,v=>v.DIC_NAME));
            return keyData;
        }
        public Dictionary<string, string> GetDicByType(string type)
        {
            var keyData = CacheService.GetFromCacheOrProxy<Dictionary<string, string>>(
              type,
              () => GetQbDicType(type).ToDictionary(k => k.DIC_CODE, v => v.DIC_NAME));
            return keyData;
        }
    }
}
