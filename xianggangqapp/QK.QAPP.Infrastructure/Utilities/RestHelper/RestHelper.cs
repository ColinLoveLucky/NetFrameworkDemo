using Microsoft.Http;
using Newtonsoft.Json;
using QK.QAPP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace QK.QAPP.Infrastructure
{
    public class RestHelper
    {
        /// <summary>
        /// REST服务URL
        /// </summary>
        public string RestUrl { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="basicURL">REST服务URL</param>
        public RestHelper(string basicURL)
        {
            RestUrl = basicURL;
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="global"></param>
        /// <returns></returns>
        public DtoMessage<T> Get<T>(string global, Dictionary<string, string> dic)
        {
            DtoMessage<T> ret = new DtoMessage<T>();
            try
            {
                HttpClient client = new HttpClient();

                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                client.DefaultHeaders.ContentType = "application/x-www-form-urlencoded";
                string strUrl = RestUrl;
                if (!string.IsNullOrEmpty(global))
                {
                    strUrl = RestUrl + "/" + global;
                }
                if (dic != null && dic.Count() > 0)
                {
                    strUrl += "?";
                    strUrl += GetUrlParam(dic);
                }
                HttpResponseMessage response = client.Get(strUrl);
                string json = response.Content.ReadAsString();

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    if (!string.IsNullOrEmpty(json))
                    {
                        if (typeof (T).Equals(typeof (string)))
                        {
                            ret.ReturnObj = (T) Convert.ChangeType(json, typeof (T));
                            ret.Status = DtoMessageStatus.Success;
                        }
                        else
                        {
                            ret = JsonConvert.DeserializeObject<DtoMessage<T>>(ClearStr(json));
                        }
                    }
                }
                else
                {
                    ret.Status = DtoMessageStatus.Fail;
                    ret.Error = "服务器通信出现异常,状态码为:" + response.StatusCode;
                }

            }
            catch (Exception ex)
            {
                ret = new DtoMessage<T>()
                {
                    Status = DtoMessageStatus.Fail,
                    ReturnObj = default(T),
                    Error = "服务器通信出现异常,详细信息:" + ex.Message
                };
            }
            return ret;
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="global"></param>
        /// <returns></returns>
        public DtoMessage<T> Get<T>(string global) where T : new()
        {
            DtoMessage<T> ret = new DtoMessage<T>();
            try
            {

                HttpClient client = new HttpClient();

                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                client.DefaultHeaders.ContentType = "application/x-www-form-urlencoded";
                string strUrl = RestUrl;
                if (!string.IsNullOrEmpty(global))
                {
                    strUrl = RestUrl + "/" + global;
                }
                HttpResponseMessage response = client.Get(strUrl);
                string json = response.Content.ReadAsString();

                if (!string.IsNullOrEmpty(json))
                {
                    //ret = JsonConvert.DeserializeObject<DtoMessage<T>>(ClearStr(json));
                    var ro = JsonConvert.DeserializeObject<T>(ClearStr(json));
                    ret = new DtoMessage<T>()
                    {
                        ReturnObj = ro, 
                        Status = DtoMessageStatus.Success
                    };

                    //if (ret.ReturnObj == null)
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {
                ret = new DtoMessage<T>()
                {
                    Status = DtoMessageStatus.Fail,
                    ReturnObj = new T(),
                    Error = "服务器通信出现异常" + ",详细信息:" + ex.Message
                };
            }
            return ret;
        }

        public DtoMessage<T> Post<T>(string global, object item) where T : new()
        {
            return Post<T>(global, item == null ? null : GetJsonParam(item), true);
        }

        public DtoMessage<T> Post<T>(string global, Dictionary<string, string> item) where T : new()
        {
            return Post<T>(global, item == null ? null : GetUrlParam(item), false);
        }

        /// <summary>
        /// Post后返回一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="global"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private DtoMessage<T> Post<T>(string global, string item, bool isJson) where T : new()
        {
            DtoMessage<T> ret = new DtoMessage<T>();
            try
            {
                HttpClient client = new HttpClient();
                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                client.DefaultHeaders.Accept.Add(isJson ? new Microsoft.Http.Headers.StringWithOptionalQuality("application/json") :
                    new Microsoft.Http.Headers.StringWithOptionalQuality("application/x-www-form-urlencoded"));
                HttpResponseMessage response = client.Post(RestUrl + global, item == null ? null : GetContent(item, isJson));
                response.EnsureStatusIsSuccessful();
                string json = response.Content.ReadAsString();
                if (!string.IsNullOrEmpty(json))

                {
                    if (typeof(T).Equals(typeof(string)))
                    {
                        ret.ReturnObj = (T)Convert.ChangeType(json, typeof(T));
                        ret.Status = DtoMessageStatus.Success;
                    }

                    ret = JsonConvert.DeserializeObject<DtoMessage<T>>(ClearStr(json));
                    if (ret.ReturnObj == null)
                    {
                        ret.ReturnObj = JsonConvert.DeserializeObject<T>(ClearStr(json));
                        if (ret.ReturnObj != null)
                        {
                            ret.Status = DtoMessageStatus.Success;
                        }
                    }
                }
                return ret;
            }
            catch
            {
                return new DtoMessage<T>() { Status = DtoMessageStatus.Fail, ReturnObj = new T(), Error = "返回数据错误" };
            }
        }

        #region 暂时没用
        public bool Post(string global, object item)
        {
            return Post(global, item == null ? null : GetJsonParam(item), true);
        }

        public bool Post(string global, Dictionary<string, string> item)
        {
            return Post(global, item == null ? null : GetUrlParam(item), false);
        }

        /// <summary>
        /// Post一个对象
        /// </summary>
        /// <param name="global"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool Post(string global, string item, bool isJson)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                HttpResponseMessage response = client.Post(RestUrl + global, item == null ? null : GetContent(item, isJson));
                response.EnsureStatusIsSuccessful();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool Put(string global, object item)
        {
            return Put(global, item == null ? null : GetJsonParam(item), true);
        }

        public bool Put(string global, Dictionary<string, string> item)
        {
            return Put(global, item == null ? null : GetUrlParam(item), false);
        }

        /// <summary>
        /// Put更新对象
        /// </summary>
        /// <param name="global"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool Put(string global, string item, bool isJson)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                HttpResponseMessage response = client.Put(RestUrl + global, item == null ? null : GetContent(item, isJson));
                response.EnsureStatusIsSuccessful();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="global"></param>
        /// <returns></returns>
        public bool Delete(string global)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                HttpResponseMessage response = client.Delete(RestUrl + global);
                response.EnsureStatusIsSuccessful();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DtoMessage<T> Put<T>(string global, object item) where T : new()
        {
            return Put<T>(global, item == null ? null : GetJsonParam(item), true);
        }

        public DtoMessage<T> Put<T>(string global, Dictionary<string, string> item) where T : new()
        {
            return Put<T>(global, item == null ? null : GetUrlParam(item), false);
        }

        /// <summary>
        /// Put后返回一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="global"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private DtoMessage<T> Put<T>(string global, string item, bool isJson) where T : new()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                client.DefaultHeaders.Accept.Add(isJson ? new Microsoft.Http.Headers.StringWithOptionalQuality("application/json") :
                    new Microsoft.Http.Headers.StringWithOptionalQuality("application/x-www-form-urlencoded"));
                HttpResponseMessage response = client.Put(RestUrl + global, item == null ? null : GetContent(item, isJson));
                response.EnsureStatusIsSuccessful();
                string json = response.Content.ReadAsString();
                return new JavaScriptSerializer().Deserialize<DtoMessage<T>>((json));
            }
            catch (Exception ex)
            {
                return new DtoMessage<T>() { Status = DtoMessageStatus.Fail, ReturnObj = new T(), Error = ex.Message };
            }
        }

        #endregion 

        private string GetJsonParam(object item)
        {
            if (item is string)
                return item as string;
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = GlobalSetting.MaxJsonLength;
            return jsSerializer.Serialize(item);
        }

        private string GetUrlParam(Dictionary<string, string> item)
        {
            return string.Join("&", item.Select(p => string.Format("{0}={1}", p.Key, HttpUtility.UrlEncode(p.Value, Encoding.UTF8))));
        }

        private HttpContent GetContent(string content, bool isJson)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(content);
            return HttpContent.Create(data, isJson ? "application/json" : "application/x-www-form-urlencoded");
        }

        private string ClearStr(string str)
        {
            str = str.Trim(new char[] { '"' });
            str = str.Replace("\\\"", "\"");
            str = str.Replace(":\":", "\":");
            str = str.Replace("\\/", "/");
            return str;

        }
    }
}
