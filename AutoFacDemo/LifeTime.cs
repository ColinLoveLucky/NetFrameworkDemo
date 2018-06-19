using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacDemo
{
    public class LifeScopeIoc
    {
        public void Write()
        {
            Console.WriteLine("LifeScopeIoc is execute!");
        }
    }
    public class LifeTimeA:IDisposable
    {
        private LifeScopeIoc _scope;
        public LifeTimeA(LifeScopeIoc scope)
        {
            _scope = scope;
        }
        public void Dispose()
        {
        }
        public void Write()
        {
            _scope.Write();
            Console.WriteLine("LifeTimeA is execute");
        }
    }
    public class LifeTime
    {
        public void TestLifeTime()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LifeTimeA>();
            builder.RegisterType<LifeScopeIoc>();
            var container = builder.Build();
            LifeTimeA instance;
            using (var scope = container.BeginLifetimeScope())
            {
                instance = container.Resolve<LifeTimeA>();
                instance.Write();
            }
            instance.Dispose();
        }
        public void TestSingleInstance()
        {

        }
        public void TestPerInstance() { }
        public void TestPerInstancePerRequest() { }
        public void TestPerInstancePerMachine() { }
        public void TestOwnInstance() { }
    }
}
