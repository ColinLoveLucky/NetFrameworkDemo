using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Security.Cryptography;
using QK.QAPP.Global;

namespace QK.QAPP.Infrastructure
{
    /// <summary>
    /// 加密算法
    /// </summary>
    public static class SecuritySignHelper
    {
        public const string Partner = "partner";
        public const string Sign = "sign";

        /// <summary>
        /// 获取包含签名的参数集合，组织原始字符串的方式为：先get后post，该签名要求partner在加密时为全小写，同时该方法隐含要求parnter和sign必须通过QueryString方式传递
        /// </summary>
        /// <param name="getCollection">通过QueryString方式传递的键值集合,如果内部包含parnter或者sign，相关字段在组织原始字符串时将会被移除</param>
        /// <param name="postCollection">通过Form方式传递的键值集合，如果包含parnter或者sign，此部分不会被做特殊处理</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetSecurityCollectionWithSign(this NameValueCollection getcollection, string partner = null, string partnerKey=null)
        {
             partner =partner?? GlobalSetting.QbPartner;// 合作账号-私钥
             partnerKey =partnerKey?? GlobalSetting.QbPartnerKey;// 合作Key-公钥

            if (string.IsNullOrWhiteSpace(partner) || string.IsNullOrWhiteSpace(partnerKey))
            {
                throw new ArgumentNullException();
            }
            var dic = SecuritySignHelper.GetDictionary(getcollection,
                (k) =>
                {//过滤partner及sign(参数为partner或者为sign无效)
                    return string.Equals(k, SecuritySignHelper.Partner, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(k, SecuritySignHelper.Sign, StringComparison.OrdinalIgnoreCase);
                });
            dic.Add(SecuritySignHelper.Partner, partner);
            StringBuilder tmp = new StringBuilder();
            SecuritySignHelper.FillStringBuilder(tmp, dic);//将集合填入StringBuilder
            tmp.Append(partnerKey);//在尾部添加key
            tmp.Remove(0, 1);//移除第一个&
            dic.Add(SecuritySignHelper.Sign, tmp.ToString().GetMD5_32());//获取32位长度的Md5摘要
            return SorteToDic(dic);
        }
        /// <summary>
        /// 获取包含签名的参数集合，组织原始字符串的方式为：先get后post，该签名要求partner在加密时为全小写，同时该方法隐含要求parnter和sign必须通过QueryString方式传递
        /// </summary>
        /// <param name="postcollection">通过Form方式传递的键值集合，如果包含parnter或者sign，此部分不会被做特殊处理</param>
        /// <param name="urlPara">post/put请求时，url参数，如果有</param>
        /// <returns></returns>
        public static Dictionary<string, string> PostSecurityCollectionWithSign(this NameValueCollection postcollection, string urlPara = "", string partner = null, string partnerKey = null)
        {
            partner = partner ?? GlobalSetting.QbPartner;// 合作账号-私钥
            partnerKey = partnerKey ?? GlobalSetting.QbPartnerKey;// 合作Key-公钥
            Dictionary<string, string> dicSecurity = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(partner) || string.IsNullOrWhiteSpace(partnerKey))
            {
                throw new ArgumentNullException();
            }
            var dic =new SortedDictionary<string, string>();
            dic.Add(SecuritySignHelper.Partner, partner);
            StringBuilder tmp = new StringBuilder();
            SecuritySignHelper.FillStringBuilder(tmp, dic);//将集合填入StringBuilder
            tmp.Insert(0, urlPara);// 如果是put方式请求，将url参数添加到加密参数的首位
            dic = GetDictionary(postcollection);
            SecuritySignHelper.FillStringBuilder(tmp, dic);//将集合填入StringBuilder
            tmp.Append(partnerKey);//在尾部添加key
            tmp.Remove(0, 1);//移除第一个&
            dicSecurity.Add(SecuritySignHelper.Partner, partner);
            dicSecurity.Add(SecuritySignHelper.Sign, tmp.ToString().GetMD5_32());//获取32位长度的Md5摘要
            return dicSecurity;
        }
        public static Dictionary<string,string> SorteToDic(SortedDictionary<string, string> sorte)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if(sorte!=null&&sorte.Count>0)
            {
                foreach (var item in sorte.Keys)
                {
                    if (item!=Partner && item!=Sign)
                    {
                        dic.Add(item, sorte[item]);
                    }
                }
                if (sorte.Keys.Contains(Partner) && sorte.Keys.Contains(Sign))
                {
                    dic.Add(Partner, sorte[Partner]);
                    dic.Add(Sign, sorte[Sign]);
                }
              
            }
            return dic;
        }
        public static SortedDictionary<string, string> GetDictionary(NameValueCollection collection, Func<string, bool> filter = null)
        {//获取排序的键值对
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            if (collection != null && collection.Count > 0)
            {
                foreach (var k in collection.AllKeys)
                {
                    if (filter == null || !filter(k))
                    {//如果没设置过滤条件 或者 （无需过滤filter(k)==false）
                        dic.Add(k, collection[k]);
                    }
                }
            }
            return dic;
        }
        private static void FillStringBuilder(StringBuilder builder, SortedDictionary<string, string> dic)
        {
            foreach (var kv in dic)
            {
                builder.Append('&');
                builder.Append(kv.Key);
                builder.Append('=');
                builder.Append(kv.Value);
            }//按key顺序组织字符串
        }

        /// <summary>
        /// 获取32位长度的Md5摘要
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetMD5_32(this string inputStr, Encoding encoding = null)
        {

            byte[] result = Encoding.Default.GetBytes(inputStr);    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本
        }
    }
}
