using Microsoft.Http;
using Newtonsoft.Json;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Log4Net;
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
    public class RestApiHelper
    {
        /// <summary>
        /// REST服务URL
        /// </summary>
        public string RestUrl { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="basicURL">REST服务URL</param>
        public RestApiHelper(string basicURL)
        {
            RestUrl = basicURL;
        }

        #region =>Get方法
        /// <summary>
        /// Get方法，返回一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="global"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public T Get<T>(string global, Dictionary<string, string> dic)
        {
            T ret = default(T);
            string strUrl = RestUrl;
            try
            {
                HttpClient client = new HttpClient();

                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                client.DefaultHeaders.ContentType = "application/x-www-form-urlencoded";
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
                //暂时定义只会返回，HttpStatusCode.OK和HttpStatusCode.InternalServerError
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (!string.IsNullOrEmpty(json))
                        {
                            if (typeof(T).Equals(typeof(string)))
                            {
                                ret = (T)Convert.ChangeType(ClearStr(json), typeof(T));
                            }
                            else
                            {
                                ret = JsonConvert.DeserializeObject<T>(ClearStr(json));
                            }
                        }
                        break;
                    case HttpStatusCode.InternalServerError:
                        //记录日志  此时json保存的是错误信息
                        LogWriter.Biz("接口内部错误", strUrl, json);
                        break;
                    case HttpStatusCode.BadRequest:
                        //记录日志  此时json保存的是错误信息
                        LogWriter.Biz("接口请求无效", strUrl, json);
                        break;
                    default:
                        ret = default(T);
                        break;

                }
            }
            catch (Exception ex)
            {
                ret = default(T);
                LogWriter.Error("接口服务器通信异常,请求地址：" + strUrl, ex);
            }
            return ret;
        }
        #endregion 

        #region =>Post方法
        /// <summary>
        /// json parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="global"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public T Post<T>(string global, object item)
        {
            return Post<T>(global, item == null ? null : GetJsonParam(item), true);
        }
        /// <summary>
        /// url patameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="global"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public T Post<T>(string global, Dictionary<string, string> item)
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
        private T Post<T>(string global, string item, bool isJson)
        {
            T ret = default(T);
            try
            {
                string strUrl = RestUrl;
                if (!string.IsNullOrEmpty(global))
                {
                    strUrl = RestUrl + "?" + global;
                }
                HttpClient client = new HttpClient();
                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                client.DefaultHeaders.Accept.Add(isJson ? new Microsoft.Http.Headers.StringWithOptionalQuality("application/json") :
                    new Microsoft.Http.Headers.StringWithOptionalQuality("application/x-www-form-urlencoded"));
                HttpResponseMessage response = client.Post(strUrl, item == null ? null : GetContent(item, isJson));
                response.EnsureStatusIsSuccessful();
                string json = response.Content.ReadAsString();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (!string.IsNullOrEmpty(json))
                        {
                            if (typeof(T).Equals(typeof(string)))
                            {
                                ret = (T)Convert.ChangeType(ClearStr(json), typeof(T));
                            }
                            else
                            {
                                ret = JsonConvert.DeserializeObject<T>(ClearStr(json));
                            }
                        }
                        break;
                    case HttpStatusCode.InternalServerError:
                         //记录日志  此时json保存的是错误信息
                        LogWriter.Biz("接口内部错误", RestUrl, json);
                        break;
                    case HttpStatusCode.BadRequest:
                        //记录日志  此时json保存的是错误信息
                        LogWriter.Biz("接口请求无效", RestUrl, json);
                        break;
                    default:
                        ret = default(T);
                        break;
                }
            }
            catch (Exception ex)
            {
                ret = default(T);
                LogWriter.Error("接口服务器通信异常,请求地址：" + RestUrl, ex);
            }
            return ret;
        }

        #endregion 

        #region =>Put方法
        public T Put<T>(string global, object item)
        {
            return Put<T>(global, item == null ? null : GetJsonParam(item), true);
        }

        public T Put<T>(string global, Dictionary<string, string> item)
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
        private T Put<T>(string global, string item, bool isJson)
        {
            T ret = default(T);
            try
            {
                //HttpClient client = new HttpClient();
                //client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                //HttpResponseMessage response = client.Put(RestUrl + global, item == null ? null : GetContent(item, isJson));
                //response.EnsureStatusIsSuccessful();
                string strUrl = RestUrl;
                if (!string.IsNullOrEmpty(global))
                {
                    strUrl = RestUrl + "?" + global;
                }
                HttpClient client = new HttpClient();
                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                client.DefaultHeaders.Accept.Add(isJson ? new Microsoft.Http.Headers.StringWithOptionalQuality("application/json") :
                    new Microsoft.Http.Headers.StringWithOptionalQuality("application/x-www-form-urlencoded"));
                HttpResponseMessage response = client.Put(strUrl, item == null ? null : GetContent(item, isJson));
                response.EnsureStatusIsSuccessful();
                string json = response.Content.ReadAsString();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (!string.IsNullOrEmpty(json))
                        {
                            if (typeof(T).Equals(typeof(string)))
                            {
                                ret = (T)Convert.ChangeType(ClearStr(json), typeof(T));
                            }
                            else
                            {
                                ret = JsonConvert.DeserializeObject<T>(ClearStr(json));
                            }
                        }
                        break;
                    case HttpStatusCode.InternalServerError:
                        //记录日志  此时json保存的是错误信息
                        LogWriter.Biz("接口内部错误", RestUrl, json);
                        break;
                    case HttpStatusCode.BadRequest:
                        //记录日志  此时json保存的是错误信息
                        LogWriter.Biz("接口请求无效", RestUrl, json);
                        break;
                    default:
                        ret = default(T);
                        break;
                }
            }
            catch (Exception ex)
            {
                ret = default(T);
                LogWriter.Error("接口服务器通信异常,请求地址：" + RestUrl, ex);
            }
            return ret;
        }
        #endregion

        #region =>Delete方法
        public T Delete<T>(string global, Dictionary<string, string> dic)
        {
            T ret = default(T);
            string strUrl = RestUrl;
            try
            {
                HttpClient client = new HttpClient();

                client.TransportSettings.Credentials = System.Net.CredentialCache.DefaultCredentials;
                client.DefaultHeaders.ContentType = "application/x-www-form-urlencoded";
                if (!string.IsNullOrEmpty(global))
                {
                    strUrl = RestUrl + "/" + global;
                }
                if (dic != null && dic.Count() > 0)
                {
                    strUrl += "?";
                    strUrl += GetUrlParam(dic);
                }
                HttpResponseMessage response = client.Delete(strUrl);
                string json = response.Content.ReadAsString();
                //暂时定义只会返回，HttpStatusCode.OK和HttpStatusCode.InternalServerError
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (!string.IsNullOrEmpty(json))
                        {
                            if (typeof(T).Equals(typeof(string)))
                            {
                                ret = (T)Convert.ChangeType(ClearStr(json), typeof(T));
                            }
                            else
                            {
                                ret = JsonConvert.DeserializeObject<T>(ClearStr(json));
                            }
                        }
                        break;
                    case HttpStatusCode.InternalServerError:
                        //记录日志  此时json保存的是错误信息
                        LogWriter.Biz("接口内部错误", strUrl, json);
                        break;
                    case HttpStatusCode.BadRequest:
                        //记录日志  此时json保存的是错误信息
                        LogWriter.Biz("接口请求无效", strUrl, json);
                        break;
                    default:
                        ret = default(T);
                        break;

                }
            }
            catch (Exception ex)
            {
                ret = default(T);
                LogWriter.Error("接口服务器通信异常,请求地址：" + strUrl, ex);
            }
            return ret;
        }
        #endregion 

        #region =>私有方法
        private string GetJsonParam(object item)
        {
            if (item is string)
                return item as string;
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = GlobalSetting.Con_MaxJsonLength;
            return jsSerializer.Serialize(item);
        }
        
        public string GetUrlParam(Dictionary<string, string> item)
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
            return str;

        }
        #endregion
    }
}
