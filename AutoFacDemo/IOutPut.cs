using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacDemo
{
    public interface IOutPut
    {
        void Write(string content);
    }

    public class ConsoleOutPut : IOutPut
    {
        public void Write(string content)
        {
            Console.WriteLine(content);
        }
    }

    public interface IDateWriter
    {
        void WriteDate();
    }

    public class TodayWriter : IDateWriter
    {
        private IOutPut _outPut;

        public TodayWriter(IOutPut outPut)
        {
            _outPut = outPut;
        }

        public void WriteDate()
        {
            _outPut.Write(DateTime.Now.ToShortDateString());
        }
    }

    public class TestOutPut
    {
        private static IContainer Container { get; set; }
        public void Test()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleOutPut>().As<IOutPut>();
            builder.RegisterType<TodayWriter>().As<IDateWriter>();
            Container = builder.Build();

            var writer = Container.Resolve<IDateWriter>();

            writer.WriteDate();
        }

        public static void WriteDate()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var writer = scope.Resolve<IDateWriter>();
                writer.WriteDate();
            }
        }
    }

}
