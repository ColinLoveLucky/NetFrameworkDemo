using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacDemo
{
    public interface IApp
    {
        void Write();
    }
    public class AndironApp : IApp
    {
        public void Write()
        {
            Console.WriteLine("Andrion App is Execute");
        }
    }
    public class AppleApp : IApp
    {
        public void Write()
        {
            Console.WriteLine("Apple App is execute");
        }
    }
    public class AssemblyIoc
    {
        public void Test()
        {
            var builder = new ContainerBuilder();
            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess).Where(x => x.Name.EndsWith("App")).Except<AppleApp>()
                   .AsImplementedInterfaces().AsSelf();
           
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var appleApp = container.Resolve<IApp>();
                appleApp.Write();
            }
        }
    }
    public class AComponent
    {
        public void Wirte()
        {
            Console.WriteLine("AComponent is execute!");
        }
    }
    public class BComponent
    {
        public void Write()
        {
            Console.WriteLine("BComponent is execute!");
        }
    }
    public class AdModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AComponent()).As<AComponent>();
        }
    }
    public class BdModule:Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new BComponent()).As<BComponent>();
        }
    }
    public class ModuleIoc
    {
        public void Test()
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyModules(assembly);
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                AComponent componet = null;
                container.TryResolve<AComponent>(out componet);
                var acomoent = container.Resolve<AComponent>();
                acomoent.Wirte();
            }
        }
    }
}
