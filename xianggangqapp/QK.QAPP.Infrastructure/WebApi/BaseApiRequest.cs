using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QK.QAPP.Infrastructure
{
    public class BaseApiRequest : IApiRequest 
    {
        public string Version = "0.0.1";

        public string AppKey { get; set; }

        public string TimeStamp { get; set; }

        public string Sign { get; set; }

        /// <summary>
        /// 系统参数签名
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        public virtual void SignRequest(string appKey, string appSecret)
        {
            AppKey = appKey;
            TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Sign = VerifyHelper.SingRequest(GetSysParamDic(), appSecret);
        }

        /// <summary>
        /// 获取系统参数字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSysParamDic()
        {
            var dic = new Dictionary<string, string>
           {
               {"Version",Version},
               {"AppKey",AppKey},
               {"TimeStamp",TimeStamp}
           };

            return dic;
        }

        /// <summary>
        /// 获取加密签名
        /// </summary>
        /// <returns></returns>
        public string GetSign()
        {
            return Sign;
        }

        /// <summary>
        /// 获取接口地址
        /// </summary>
        /// <returns></returns>
        public string GetApiName()
        {
            return string.Empty;
        }
    }
}