using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacDemo
{
    public interface ILogger
    {

    }
    public class ConsoleLogger : ILogger { }
    public interface IConfigReader
    {
        void Write();
    }
    public class ConfigureReader : IConfigReader
    {
        private string _configSectionName;
        public ConfigureReader(string configSectionName)
        {
            _configSectionName = configSectionName;
        }
        public void Write()
        {
            Console.WriteLine("{0}", _configSectionName);
        }
    }
    public class WebReader : IConfigReader
    {
        public void Write()
        {
            throw new NotImplementedException();
        }
    }
    public class AppReader : IConfigReader
    {
        public void Write()
        {
            throw new NotImplementedException();
        }
    }
    public class MyComponent
    {
        private ILogger _logger;
        private IConfigReader _configReader;
        public MyComponent() { }
        public MyComponent(ILogger logger)
        {
            _logger = logger;
        }
        public MyComponent(ILogger logger, IConfigReader reader)
        {
            _logger = logger;
            _configReader = reader;
        }
        public void Wirte()
        {
            if (_configReader != null && _logger != null)
                Console.WriteLine("I'm ConfigReader Init");
            if (_logger != null)
                if (_configReader == null)
                    Console.WriteLine("I'm Logger Init");
            if (_logger == null && _configReader == null)
                Console.WriteLine("I'm Default Init");
        }
    }
    public class TestMyComponent
    {
        public void Test()
        {
            var builder = new ContainerBuilder();
            // builder.RegisterType<MyComponent>();
            builder.RegisterType<ConsoleLogger>().As<ILogger>();
            builder.RegisterType<WebReader>().As<IConfigReader>().PreserveExistingDefaults();
            // builder.RegisterType<AppReader>().As<IConfigReader>();
            // builder.RegisterType<MyComponent>().UsingConstructor(typeof(ILogger), typeof(IConfigReader));
            builder.RegisterType<MyComponent>()
             .OnlyIf(reg =>
            reg.IsRegistered(new TypedService(typeof(ILogger))) &&
            reg.IsRegistered(new TypedService(typeof(IConfigReader))));
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var component = container.Resolve<MyComponent>();
                component.Wirte();
            }
        }
        public void TestInstance()
        {
            var builder = new ContainerBuilder();
            StringBuilder content = new StringBuilder();
            var output = new StringWriter(content);
            builder.RegisterInstance(output).As<TextWriter>().ExternallyOwned();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var writer = container.Resolve<TextWriter>();
                writer.WriteLine("Hello World");
                writer.Flush();
                Console.Write(content.ToString());
            }
        }
        public void TestParameter()
        {
            var builder = new ContainerBuilder();
            builder.Register<IConfigReader>(
            (c, p) =>
            {
                var accountId = p.Named<string>("accountId");
                if (accountId.StartsWith("12345"))
                {
                    return new WebReader();
                }
                else
                {
                    return new AppReader();
                }
            });
            var container = builder.Build();
            var reader = container.Resolve<IConfigReader>(new NamedParameter("accountId", "12345"));
            if (reader is WebReader)
                Console.WriteLine("WebReader is execute!");
            if (reader is AppReader)
                Console.WriteLine("AppReader is execute!");
        }
        public void TestConstureParam()
        {
            var builder = new ContainerBuilder();
            //builder.Register(c => new ConfigureReader("Hello World")).
            //    As<IConfigReader>();
            //builder.RegisterType<ConfigureReader>().As<IConfigReader>()
            //    .WithParameter("configSectionName", "Hello World");
            //builder.RegisterType<ConfigureReader>().As<IConfigReader>().
            //    WithParameter(new TypedParameter(typeof(string), "Hello World"));
            //builder.RegisterType<ConfigureReader>().As<IConfigReader>().
            //    WithParameter(new ResolvedParameter(
            //        (x, y) => x.ParameterType == typeof(string) && x.Name == "configSectionName", (x, y) => "Hello World"));
            //builder.Register((c, p) =>
            //     new ConfigureReader("Hello World"))
            //        .As<IConfigReader>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var reader = container.Resolve<IConfigReader>();
                reader.Write();
            }
        }
    }

}
