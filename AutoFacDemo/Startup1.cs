using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Autofac;

[assembly: OwinStartup(typeof(AutoFacDemo.Startup1))]

namespace AutoFacDemo
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MyCustomerMiddleware>();
            var container = builder.Build();
            ///Autofac.Owin
            app.UseAutofacMiddleware(container);
        }
    }
}
