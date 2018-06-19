using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    public class AppAccountBiz : Singleton<AppAccountBiz>
    {
        public bool Validate(string sign, Dictionary<string, string> sysParamDic)
        {
            sysParamDic["AppKey"] = GetAppKey();
            var secret = GetAppSecret();
            var isValidate = sign == VerifyHelper.SingRequest(sysParamDic, secret);
            return isValidate;
        }

        /// <summary>
        /// 获取AppSecret
        /// </summary>
        /// <returns></returns>
        public string GetAppSecret()
        {
            return "d1f8f0a29c12c3998395a747fbc1ad24";
        }

        /// <summary>
        /// 获取AppKey
        /// </summary>
        /// <returns></returns>
        public string GetAppKey()
        {
            return "1035541";
        }
    }
}
