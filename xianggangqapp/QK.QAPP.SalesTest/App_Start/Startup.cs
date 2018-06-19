using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;
using QK.QAPP.SalesTest.Hubs;
using System;

[assembly: OwinStartup(typeof(QK.QAPP.SalesTest.Startup))]
namespace QK.QAPP.SalesTest
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
            //GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}
