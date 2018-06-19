using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacDemo
{
    public class MyCustomerMiddleware
    {
        public void Write()
        {
            Console.WriteLine("MyCustomerMiddleware is execute!");
        }
    }

    public class AutoFacOwin
    {
        public void Test()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var middleware = scope.Resolve<MyCustomerMiddleware>();
                middleware.Write();
            }
        }
    }
}
