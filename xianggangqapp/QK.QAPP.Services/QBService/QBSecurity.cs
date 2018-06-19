using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Utilities;
using System.Collections.Specialized;

namespace QK.QAPP.Services.QBService
{
    public class QBSecurity:IQBSecurity
    {
        public string GetSecurityUrl()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取防篡改签名，组织原始字符串的方式为：先get后post，该签名要求partner在加密时为全小写，同时该方法隐含要求parnter和sign必须通过QueryString方式传递
        /// </summary>
        /// <param name="getCollection">通过QueryString方式传递的键值集合,如果内部包含parnter或者sign，相关字段在组织原始字符串时将会被移除</param>
        /// <param name="partner">合作账号</param>
        /// <param name="partnerKey">合作Key</param>
        /// <param name="postCollection">通过Form方式传递的键值集合，如果包含parnter或者sign，此部分不会被做特殊处理</param>
        
        private string GetSecuritySign(NameValueCollection getCollection, string Partner, string PartnerKey, NameValueCollection postCollection)
        {
            return SecuritySignHelper.GetSecuritySign(getCollection, Partner, PartnerKey, postCollection);
        }
        /// <summary>
        /// 获取防篡改签名，组织原始字符串的方式为：post
        /// </summary>
        /// <param name="Partner"></param>
        /// <param name="PartnerKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
        private string GetSecuritySign(string Partner, string PartnerKey, NameValueCollection postCollection)
        {
            NameValueCollection getCollection = null;
            return GetSecuritySign(getCollection, Partner, PartnerKey, postCollection);
        }
        /// <summary>
        /// 获取防篡改签名，组织原始字符串的方式为：get
        /// </summary>
        /// <param name="getCollection"></param>
        /// <param name="Partner"></param>
        /// <param name="PartnerKey"></param>
        /// <returns></returns>
        private string GetSecuritySign(NameValueCollection getCollection, string Partner, string PartnerKey)
        {
            NameValueCollection postCollection = null;
            return GetSecuritySign(getCollection, Partner, PartnerKey, postCollection);
        }
        /// <summary>
        /// 获取接口参数
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="Sign"></param>
        /// <param name="isSign"></param>
        /// <returns></returns>
        private string GetUrlParams(NameValueCollection collection,string Sign="",bool isSign=true)
        {
            if (isSign)
            {
                collection.Add(SecuritySignHelper.Partner, GlobalSetting.QbPartner);
                collection.Add(SecuritySignHelper.Sign, Sign);
            }
            StringBuilder tmp = new StringBuilder();
            for (int i = 0; i < collection.Count; i++)
            {
                tmp.Append('&');
                tmp.Append(collection.GetKey(i));
                tmp.Append('=');
                tmp.Append(collection[i]);
            }
            tmp.Remove(0, 1);
            return tmp.ToString();
        }
    }
}
