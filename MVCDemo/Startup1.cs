using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(MVCDemo.Startup1))]

namespace MVCDemo
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
         
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
