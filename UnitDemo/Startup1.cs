using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(UnitDemo.Startup1))]

namespace UnitDemo
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello, world.");
            });
        }
    }
}
