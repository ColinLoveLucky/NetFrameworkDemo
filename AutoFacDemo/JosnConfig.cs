using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;

namespace AutoFacDemo
{
    public interface IJson
    {
        void Write();
    }
    public class JosnConfig : IJson
    {
        public void Write()
        {
            Console.WriteLine("JsonConfig is execute!");
        }
    }
    public class JsonApp
    {
        private IJson _json;

        public JsonApp(IJson json)
        {
            _json = json;
        }

        public void OutPut()
        {
            _json.Write();
            Console.WriteLine("JsonIoc is execute!");
        }
    }
    public class JsonUnit
    {
        public void Test()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile(@"D:\FrameWork4.0TestDemo\AutoFacDemo\autoFac.json");
            var module = new ConfigurationModule(config.Build());
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var jsonConfig = scope.Resolve<IJson>();
                jsonConfig.Write();
            }
        }

        public void TestModule()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile(@"D:\FrameWork4.0TestDemo\AutoFacDemo\autoFac.json");
            var module = new ConfigurationModule(config.Build());
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var jsonConfig = scope.Resolve<BComponent>();
                jsonConfig.Write();
            }
        }
    }
    public class XmlUnit
    {
        /// <summary>
        /// VerPre 4.0 之前 用XML配置，之后用json配置了
        /// </summary>
        public void Test()
        {
            //var config = new ConfigurationBuilder();
            //config.AddXmlFile(@"D:\FrameWork4.0TestDemo\AutoFacDemo\XmlConfig.xml");
            //var module = new ConfigurationModule(config.Build());
            //var builder = new ContainerBuilder();
            //builder.RegisterModule(module);
            //var container = builder.Build();
            //using (var scope = container.BeginLifetimeScope())
            //{
            //    var jsonConfig = scope.Resolve<IJson>();
            //    jsonConfig.Write();
            //}
            //ConfigurationSettingsReader
        }
    }


}
