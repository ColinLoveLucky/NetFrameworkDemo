using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    public static class WebRequestHelper
    {
        public enum Method { GET, POST };

        /// <summary>
        /// 发起web请求
        /// </summary>
        /// <param name="method">请求方式：GET，POST</param>
        /// <param name="url">请求地址</param>
        /// <param name="contentType"></param>
        /// <param name="postData">请求方式为POST时，传入请求数据</param>
        /// <param name="cookie">如果需要cookie则传入，否则传入null即可</param>
        /// <returns></returns>
        public static string Request(Method method, string url, string contentType = null, string postData = null, Cookie cookie = null)
        {
            HttpWebRequest webRequest = null;

            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.KeepAlive = true;

            if (cookie != null)
            {
                webRequest.CookieContainer = new CookieContainer();
                webRequest.CookieContainer.Add(cookie);
            }

            if (method == Method.POST)
            {
                if (string.IsNullOrEmpty(contentType))
                {
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    webRequest.ContentType = contentType;
                }

                if (!string.IsNullOrEmpty(postData))
                {
                    StreamWriter requestWriter = null;
                    requestWriter = new StreamWriter(webRequest.GetRequestStream());
                    try
                    {
                        requestWriter.Write(postData);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        requestWriter.Close();
                        requestWriter = null;
                    }
                }
            }

            responseData = WebResponseGet(webRequest);
            webRequest = null;

            return responseData;
        }

        private static string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader = null;
                }
            }
            return responseData;
        }
    }
}
