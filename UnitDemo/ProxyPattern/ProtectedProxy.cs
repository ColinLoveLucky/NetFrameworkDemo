using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ProxyPattern
{
    public interface IProtectedProxy
    {
        void Request();
    }
    public class ProtectedProxy : IProtectedProxy
    {
        private string _name;

        private IProtectedProxy _protectedProxy;

        public ProtectedProxy(string name)
        {
            _name = name;

            _protectedProxy = new ProtectedTruthInstance();
        }

        public void Request()
        {
            if (_name == "GhostBear")
                _protectedProxy.Request();
            else
                Console.WriteLine("you promision to access this method");
        }
    }
    public class ProtectedTruthInstance : IProtectedProxy
    {
        public ProtectedTruthInstance() { }
        public void Request()
        {
            Console.WriteLine("you admission to access this method");
        }
    }
    public class ProtectedProxyMain
    {
        public void Main()
        {
            ProtectedProxy pp = new ProtectedProxy("jim");

            pp.Request();

            Console.ReadKey();
        }
    }

}
