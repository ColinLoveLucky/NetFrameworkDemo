using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ProxyPattern
{
    public interface IRemoteProxy
    {
        void RemoteRequest();
    }
    public class RemoteInstance : IRemoteProxy
    {
        private string _name;

        public RemoteInstance(string name)
        {
            _name = name;
        }

        public void RemoteRequest()
        {
            Console.WriteLine(string.Format("You current request Instance is {0} and it's {1}", _name, DateTime.Now));

        }
    }
    public class RemoteProxy : IRemoteProxy
    {
        private IRemoteProxy _remoteProxy;

        public RemoteProxy(IRemoteProxy remoteProxy)
        {
            _remoteProxy = remoteProxy;
        }
        public void RemoteRequest()
        {
            _remoteProxy.RemoteRequest();
        }
    }
    public class RemoteProxyMain
    {
        public void Main()
        {
            RemoteInstance ri = new RemoteInstance("cemare");

            RemoteProxy rp = new RemoteProxy(ri);

            rp.RemoteRequest();

            Console.ReadKey();
        }
    }

}
