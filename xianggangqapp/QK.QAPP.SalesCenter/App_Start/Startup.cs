using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;
using QK.QAPP.Global;
using QK.QAPP.SalesCenter.Hubs;
using System;

[assembly: OwinStartup(typeof(QK.QAPP.SalesCenter.Startup))]
namespace QK.QAPP.SalesCenter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableJavaScriptProxies = true;
            hubConfiguration.EnableJSONP = true;
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR(hubConfiguration);

            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);
            //GlobalHost.DependencyResolver.UseRedis(GlobalSetting.CahcheServer, 
            //    GlobalSetting.CahcheServerPort,
            //    string.Empty, 
            //    "QAPPSignalr");
            GlobalHost.DependencyResolver.UseRedis(
                GlobalSetting.CahcheServer,
                GlobalSetting.CahcheServerPort,
                "",
                GlobalSetting.SignalREventKey);
            //GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}
