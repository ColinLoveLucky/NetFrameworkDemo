using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace UnitDemo.HttpCache
{
    public class HttpCacheDemo
    {
        private static Lazy<Cache> _cache = new Lazy<Cache>(false);

        public Cache Cache
        {
            get
            {
                return _cache.Value;
            }
        }
        public void Insert<T>(string key, T value, int expires)
        {

            Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, expires, 0));
        }

        public T Get<T>(string key)
        {
            return (T)Cache.Get(key);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }
    }


}
