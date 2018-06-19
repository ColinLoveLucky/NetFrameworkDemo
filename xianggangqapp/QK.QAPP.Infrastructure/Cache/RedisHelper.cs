/*********************
 * 作者：刘成帅
 * 时间：2014/11/18
 * 功能：用来操作缓存的静态类，解决缓存注入出错问题
**********************/

using QK.QAPP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.Cache
{
    public static class RedisHelper
    {

        /// <summary>
        /// 空对象 用来锁
        /// </summary>
        static object objLock = new object();
        //static ICacheProvider cp = new RedisCacheProvider(GlobalSetting.CacheSeverConfiguration);

        /// <summary>
        /// 获取所有键
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllKey()
        {
            using (ICacheProvider cp=new RedisCacheProvider(GlobalSetting.CacheSeverConfiguration))
            {
                return cp.GetALLKey();
            }            
        }

        /// <summary>
        /// 从缓存中读取相关键的值，如果不存在键从Func参数中执行函数并且存入值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="fun">需要执行的函数</param>
        /// <returns>返回对象</returns>
        public static T GetFromCacheOrProxy<T>(string key, Func<T> fun)
        {
            using (ICacheProvider cp = new RedisCacheProvider(GlobalSetting.CacheSeverConfiguration))
            {
                return cp.GetFromCacheOrProxy(key, fun);
            }
        }

        /// <summary>
        /// 从缓存中读取相关键的值，如果不存在键从Func参数中执行函数并且存入值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="fun">需要执行的函数</param>
        /// <param name="expiresAt">过期时间</param>
        /// <returns>返回对象</returns>
        public static T GetFromCacheOrProxy<T>(string key, Func<T> fun, DateTime expiresAt)
        {
            using (ICacheProvider cp = new RedisCacheProvider(GlobalSetting.CacheSeverConfiguration))
            {
                return cp.GetFromCacheOrProxy(key, fun, expiresAt);
            }
        }

        /// <summary>
        /// 从缓存中读取相关键的值，如果不存在键从Func参数中执行函数并且存入值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="fun">需要执行的函数</param>
        /// <param name="expiresIn">过期时间</param>
        /// <returns>返回对象</returns>
        public static T GetFromCacheOrProxy<T>(string key, Func<T> fun, TimeSpan expiresIn)
        {
            using (ICacheProvider cp = new RedisCacheProvider(GlobalSetting.CacheSeverConfiguration))
            {
                return cp.GetFromCacheOrProxy(key, fun, expiresIn);
            }
        }

        /// <summary>
        /// 移除某一个键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否成功</returns>
        public static bool Remove(string key)
        {
            lock (objLock)
            {
                using (ICacheProvider cp = new RedisCacheProvider(GlobalSetting.CacheSeverConfiguration))
                {
                    return cp.Remove(key);
                }
            }
        }
        /// <summary>
        /// 移除某一个以某个值开始的键
        /// </summary>
        /// <param name="key">值</param>
        /// <returns>返回移除的函数</returns>
        public static int RemoveStartWith(string key)
        {
            lock (objLock)
            {
                using (ICacheProvider cp = new RedisCacheProvider(GlobalSetting.CacheSeverConfiguration))
                {
                    return cp.RemoveStartWith(key);
                }
            }
        }

    }
}
