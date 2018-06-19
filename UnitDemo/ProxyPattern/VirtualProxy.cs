using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ProxyPattern
{
    public interface IVirtualProxy
    {
        void Request();
    }
    public class VirtualProxy : IVirtualProxy
    {
        private IVirtualProxy _truthInstance;

        private bool flag = false;

        public VirtualProxy()
        {

        }

        public void Request()
        {
            if (_truthInstance != null)
                _truthInstance.Request();
            else
            {
                if(!flag)
                {
                    flag = true;
                    new Thread(delegate()
                    {
                        _truthInstance = new TruthInstance("摄像头");
                        this.Request();
                    }).Start();
                }
            }
        }
    }
    public class TruthInstance:IVirtualProxy
    {
        private string _name;
        public TruthInstance(string name)
        {
            Thread.Sleep(5000);
            _name = name;
        }
        public void Request()
        {
            Console.WriteLine(string.Format("you current request instance is " + _name));
        }
    }
    public class VirtualProxyMain
    {
        public void Main()
        {
            VirtualProxy vr = new VirtualProxy();

            vr.Request();

            vr.Request();

            vr.Request();
        }
    }
}
