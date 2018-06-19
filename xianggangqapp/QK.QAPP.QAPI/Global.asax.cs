
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CFCA.Payment.Api;

namespace QK.QAPP.QAPI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            init();
        }

        /// <summary>
        /// 执行系统配置参数和证书信息
        /// </summary>
        private void init()
        {
            bool IsDebug = WebConfigurationManager.AppSettings["IsDebug"] == "1" ? true : false;
            String configPath = string.Empty;
            if (IsDebug)
            {
                //取得模拟测试配置文件路径
                configPath = Server.MapPath(WebConfigurationManager.AppSettings["payment.config.path.debug"]);
            }
            else
            {
                //取得正式运营配置文件路径
                configPath = Server.MapPath(WebConfigurationManager.AppSettings["payment.config.path"]);
            }
            //执行系统初始化
            PaymentEnvironment.Initialize(configPath);

        }
    }
}