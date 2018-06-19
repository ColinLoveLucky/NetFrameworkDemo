using QK.QAPP.Global;
using RedisSessionProvider.Config;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QK.QAPP.SalesTest.App_Start
{
    public class RedisSessionStateStoreProvider
    {
        /// <summary>
        /// 初始化SESSION
        /// </summary>
        public static void Init()
        {
            // assign your local Redis instance address, can be static
            var redisConfigOpts = ConfigurationOptions.Parse(string.Format("{0}:{1}",
                GlobalSetting.CahcheServer, GlobalSetting.CahcheServerPort));

            // pass it to RedisSessionProvider configuration class
            RedisConnectionConfig.GetSERedisServerConfig = (HttpContextBase context) =>
            {
                return new KeyValuePair<string, ConfigurationOptions>(
                    "DefaultConnection",				// if you use multiple configuration objects, please make the keys unique
                    redisConfigOpts);
            };
        }
             
    }
}