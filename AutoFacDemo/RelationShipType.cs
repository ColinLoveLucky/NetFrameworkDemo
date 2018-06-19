using Autofac;
using Autofac.Features.Indexed;
using Autofac.Features.Metadata;
using Autofac.Features.OwnedInstances;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoFacDemo
{

    public class A
    {
        public virtual void Write()
        {
            Console.WriteLine("A is execute!");
        }
    }
    public class B
    {
        private A _a;
        public B(A a)
        {
            _a = a;
        }
        public void Write()
        {
            _a.Write();
            Console.WriteLine("B is execute!");
        }
    }
    public class LazyC
    {
        Lazy<A> _a;

        public LazyC(Lazy<A> a)
        {
            _a = a;
        }

        public void Write()
        {
            _a.Value.Write();

            Console.WriteLine("LazyC is execute!");
        }
    }
    public class LifeTimeD
    {
        private Owned<A> _a;
        public LifeTimeD(Owned<A> a)
        {
            _a = a;
        }
        public void Write()
        {
            _a.Value.Write();
            Console.WriteLine("LifeTime is execute!");
            _a.Dispose();
        }
    }
    public class DynamicE
    {
        private Func<A> _a;

        public DynamicE(Func<A> a) { _a = a; }

        public void Write()
        {
            var a = _a();
            a.Write();
            Console.WriteLine("DynamiceE is execute!");
        }
    }
    public class DulicateTypes
    {
        private int _a;
        private int _b;
        private string _c;
        public DulicateTypes(int a, int b, string c)
        {
            _a = a;
            _b = b;
            _c = c;
        }
        public void Write()
        {
            Console.WriteLine("a:{0},b:{1},c:{2}", _a, _b, _c);
        }
    }
    public class ParmeterizedF
    {
        private Func<int, string, A> _a;
        public ParmeterizedF(Func<int, string, A> a) { _a = a; }

        public void Write()
        {
            var a = _a(42, "Hello World");
            a.Write();
            Console.WriteLine("ParameterizedF is execute!");
        }
    }
    public class Message
    {
        public void Write()
        {
            Console.WriteLine("Hello World");
        }
    }
    public interface IMessageHandler
    {
        void HandleMessage(Message m);
    }
    public class FirstHanlder : IMessageHandler
    {
        public void HandleMessage(Message m)
        {
            Console.WriteLine("First Handler is execute!");
        }
    }
    public class SencodHandler : IMessageHandler
    {
        public void HandleMessage(Message m)
        {
            Console.WriteLine("Second handler is execute");
        }
    }
    public class MessageProcessor
    {
        private IEnumerable<IMessageHandler> _handlers;
        public MessageProcessor(IEnumerable<IMessageHandler> hanlders)
        {
            _handlers = hanlders;
        }
        public void ProcessMessage(Message m)
        {
            foreach (var handler in _handlers)
            {
                handler.HandleMessage(m);
            }
        }
    }
    public class MetaA
    {

        private Meta<A> _a;
        public MetaA(Meta<A> a) { _a = a; }
        public void Write()
        {
            if (_a.Metadata["SomeValue"].ToString() == "yes")
                _a.Value.Write();
        }
    }
    public class KeyedLookup
    {
        private IIndex<string, A> _a;
        public KeyedLookup(IIndex<string, A> a)
        {
            _a = a;
        }
        public void Write()
        {
            var a = _a["first"];
            if (a is DerivedA)
                a.Write();
        }
    }
    public class DerivedA : A
    {
        public override void Write()
        {
            Console.WriteLine("DeriveA is execute!");
        }
    }
    public class AnotherDerivedA : A
    {
        public override void Write()
        {
            Console.WriteLine("AnotherDerivedA is execute!");
        }
    }
    public class RelationShipType
    {
        public void TestDirectDependence()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<A>();
            builder.RegisterType<B>();
            var continer = builder.Build();
            using (var scope = continer.BeginLifetimeScope())
            {
                var b = continer.Resolve<B>();
                b.Write();
            }


        }
        public void TestDelayDependence()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<A>();
            builder.RegisterType<LazyC>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var c = scope.Resolve<LazyC>();
                c.Write();
            }
        }
        public void TestLifeTimeDependence()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<A>();
            builder.RegisterType<LifeTimeD>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var d = scope.Resolve<LifeTimeD>();
                d.Write();


            }
        }
        public void TestDynaimcDependunce()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<A>();
            builder.RegisterType<DynamicE>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var e = scope.Resolve<DynamicE>();
                e.Write();
            }
        }
        public void TestParamterized()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<A>();
            builder.RegisterType<ParmeterizedF>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var e = scope.Resolve<ParmeterizedF>();
                e.Write();
            }
        }
        private delegate DulicateTypes FactoryDelegate(int a, int b, string c);
        public void TestDulicateTypes()
        {
            //var builder = new ContainerBuilder();
            //builder.RegisterType<DulicateTypes>();
            //builder.RegisterDecorator<FactoryDelegate>(new TypedService(typeof(DulicateTypes)));
            //var container = builder.Build();
            //using (var scope = container.BeginLifetimeScope())
            //{
            //    var e = scope.Resolve<ParmeterizedF>();
            //    e.Write();
            //}
        }
        public void TestEnumerable()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FirstHanlder>().As<IMessageHandler>();
            builder.RegisterType<SencodHandler>().As<IMessageHandler>();
            builder.RegisterType<MessageProcessor>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var processor = scope.Resolve<MessageProcessor>();
                processor.ProcessMessage(new Message());
            }
        }
        public void TestMeta()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<A>().WithMetadata("SomeValue", "yes");
            builder.RegisterType<MetaA>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var e = scope.Resolve<MetaA>();
                e.Write();
            }
        }
        public void IIndex()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DerivedA>().Keyed<A>("first");
            builder.RegisterType<AnotherDerivedA>().Keyed<A>("second");
            builder.RegisterType<KeyedLookup>();
            var container = builder.Build();
            KeyedLookup e;
            using (var scope = container.BeginLifetimeScope())
            {
                e = scope.Resolve<KeyedLookup>();
                e.Write();
            }
        }
    }
}
