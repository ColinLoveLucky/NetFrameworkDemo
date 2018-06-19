using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacDemo
{
    public interface IMedia
    {

    }
    public class Video : IMedia { }
    public class NullMedia : IMedia
    {
        public static IMedia Instance
        {
            get
            {
                return new NullMedia();
            }
        }
    }
    public interface ICaller
    {
        void Wirte();
    }
    public class DBCaller : ICaller
    {
        public IMedia Media { get; set; }
        public DBCaller()
        {
            Media = NullMedia.Instance;
        }
        public void Wirte()
        {
            if (Media != null)
            {
                if (Media is NullMedia)
                    Console.WriteLine("NULL Media is execute!");
                else if (Media is Video)
                    Console.Write("Video media is execute!");
            }
            else
                Console.WriteLine("Media is not init!");
        }
    }
    public class PropertyIoc
    {
        public void Test()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Video>().As<IMedia>();
            //builder.Register(c => new DBCaller { Media = c.Resolve<IMedia>() }).As<ICaller>();
            //builder.RegisterType<DBCaller>().PropertiesAutowired().As<ICaller>();
            //builder.RegisterType<DBCaller>().WithProperty("Media", new Video()).As<ICaller>();
            builder.RegisterType<DBCaller>().OnActivating(e => e.Instance.Media = e.Context.Resolve<IMedia>()).As<ICaller>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var caller = container.Resolve<ICaller>();
                caller.Wirte();
            }
        }

        public void TestMethod()
        {
        }
    }
}
