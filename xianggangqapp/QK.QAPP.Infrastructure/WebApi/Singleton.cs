using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    /// <summary>
    /// 禁止在继承此类的任何类，使用成员变量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : class,new()
    {
        private static T _instance;

        private static object SynRoot = new object();
        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SynRoot)
                {
                    if (_instance != null)
                        return _instance;

                    return _instance = new T();
                }
            }
        }
    }
}
