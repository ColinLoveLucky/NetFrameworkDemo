using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Log4Net;

namespace QK.QAPP.Services
{
    public partial class APP_GLOBALCONFIGSERVICE
    {
        /// <summary>
        /// 通过键获取系统配置值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public string GetValueByKey(string key)
        {
            var caheService = Ioc.GetService<ICacheProvider>();
            var configValue = caheService.GetFromCacheOrProxy("QAPP_GLOBALCONFIG_" + key, () =>
            {
                var obj = this.FirstOrDefault(c => c.KEY == key);
                if (obj != null)
                {
                    return obj.VALUE;
                }
                //System.Diagnostics.Trace.Write("未取到系统配置[" + key + "]的值！", "error");
                LogWriter.Warn("未取到系统配置[" + key + "]的值！");
                return "";
            });
            if (string.IsNullOrEmpty(configValue))
            {
                System.Diagnostics.Trace.TraceWarning("取到系统配置[" + key + "]的值为空！");
            }
            return configValue;


        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        /// <param name="key"></param>
        public void ClearCacheByKey(string key)
        {
            var caheService = Ioc.GetService<ICacheProvider>();
            if (caheService.Contains("QAPP_GLOBALCONFIG_" + key))
            {
                caheService.Remove("QAPP_GLOBALCONFIG_" + key);
            }
        }

    }
}
