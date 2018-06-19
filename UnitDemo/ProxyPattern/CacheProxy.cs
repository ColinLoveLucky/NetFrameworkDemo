using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ProxyPattern
{
    public interface ICacheProxy
    {
        void Request();
    }
    public class CacheProxy : ICacheProxy
    {
        private ICacheProxy _cacheProxy;

        private string _name;

        private IDictionary<string, string> _cacheName = new Dictionary<string, string>();

        public CacheProxy(string name)
        {
            _name = name;

            _cacheName.Add(name, name);
        }

        public void Request()
        {
           if(_cacheName.ContainsKey(_name))
           {
               _cacheProxy = new CacheProxyInstacne(_cacheName[_name]);

               _cacheProxy.Request();
           }
           else
           {
              // _cacheProxy=new CacheProxyInstacne()
           }
        }
    }
    public class CacheProxyInstacne : ICacheProxy
    {
        private string _name;

        public CacheProxyInstacne(string name)
        {
            _name = name;
        }

        public void Request()
        {
            Console.WriteLine(string.Format("name is cache{0}",_name));
        }
    }

}
