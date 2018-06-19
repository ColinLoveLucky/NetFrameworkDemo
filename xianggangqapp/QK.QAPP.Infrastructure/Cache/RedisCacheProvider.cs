using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QK.QAPP.Infrastructure.Cache
{
    /// <summary>
    /// Redis缓存提供者
    /// </summary>
    public class RedisCacheProvider : ICacheProvider
    {
        /// <summary>
        /// 多线程读写锁
        /// </summary>
        //public readonly ReaderWriterLockSlim SyncRoot = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// 客户端实例
        /// </summary>
        //RedisClient client;
        ConnectionMultiplexer Connection;
        IDatabase client;

        


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="host">地址(IP或者机器名)</param>
        /// <param name="port">端口:6379</param>
        public RedisCacheProvider(string Configuration)
        {
            if (!string.IsNullOrWhiteSpace(Configuration))
            {
                Connection = ConnectionMultiplexer.Connect(Configuration);                
                client = Connection.GetDatabase();
            }
            else
            {
                throw new ArgumentNullException("Configuration不能为空");
            }            
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
            bool ret = false;
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key值不能为空");
            }
            try
            {
                var strJosn = JsonConvert.SerializeObject(value);
                client.StringSet(key, strJosn);
            }
            catch (Exception)
            {

            }
            finally
            {
                // SyncRoot.ExitWriteLock();
            }
            return ret;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiresAt">缓存过期时间</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            bool ret = false;
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key值不能为空");
            }
            //SyncRoot.EnterWriteLock();
            try
            {
                var Times =  expiresAt-DateTime.Now;
                var strJosn = JsonConvert.SerializeObject(value);
                client.StringSet(key, strJosn, Times);
                //client.Set<T>(key, value, expiresAt);
            }
            catch (Exception)
            {

            }
            finally
            {
                //SyncRoot.ExitWriteLock();
            }
            return ret;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="value">值</param>
        /// <param name="expiresIn">缓存过期时间段</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            bool ret = false;
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key值不能为空");
            }
            //SyncRoot.EnterWriteLock();
            try
            {
                var strJosn = JsonConvert.SerializeObject(value);
                client.StringSet(key, strJosn, expiresIn);
            }
            catch (Exception)
            {

            }
            finally
            {
                //SyncRoot.ExitWriteLock();
            }
            return ret;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>类型</returns>
        public T Get<T>(string key)
        {
            //SyncRoot.EnterWriteLock();
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key值不能为空");
            }
            var strRet = client.StringGet(key);
            var ret = JsonConvert.DeserializeObject<T>(strRet);           
            //SyncRoot.ExitWriteLock();
            return ret;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            bool ret = false;
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key值不能为空");
            }
            //SyncRoot.EnterWriteLock();
            try
            {
                ret = client.KeyDelete(key);
            }
            catch (Exception)
            {

            }
            finally
            {
                //SyncRoot.ExitWriteLock();
            }
            return ret;
        }

        /// <summary>
        /// 是否存在缓存键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key值不能为空");
            }
            return client.KeyExists(key);
        }

        /// <summary>
        /// 从缓存中获取对象，如果缓存没有，执行函数后，存入缓存后再获取
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="fun">函数</param>
        /// <returns></returns>
        public T GetFromCacheOrProxy<T>(string key, Func<T> fun)
        {
            try
            {
                if (Contains(key))
                {
                    // Redis=> 存在KEY 读取出VALUE 
                    return Get<T>(key);
                }
                else
                {
                    var obj = fun();
                    Set<T>(key, obj);
                    // Redis=> 不存在在KEY 执行函数 存入VALUE 
                    return obj;
                }
            }
            catch (Exception)
            {

                var obj = fun();
                //Set<T>(key, obj);
                // Redis=> 异常，返回执行函数后的值 
                return obj;
            }

        }
        /// <summary>
        /// 从缓存中获取对象，如果缓存没有，执行函数后，存入缓存后再获取
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="fun">函数</param>
        /// <param name="expiresAt">过期时间</param>
        /// <returns></returns>
        public T GetFromCacheOrProxy<T>(string key, Func<T> fun, DateTime expiresAt)
        {
            try
            {
                if (Contains(key))
                {
                    return Get<T>(key);
                }
                else
                {
                    var obj = fun();
                    Set<T>(key, obj, expiresAt);
                    return obj;
                }
            }
            catch
            {
                var obj = fun();
                //Set<T>(key, obj, expiresAt);
                return obj;
            }
        }
        /// <summary>
        /// 从缓存中获取对象，如果缓存没有，执行函数后，存入缓存后再获取
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="fun">函数</param>
        /// <param name="expiresAt">过期时间</param>
        /// <returns></returns>
        public T GetFromCacheOrProxy<T>(string key, Func<T> fun, TimeSpan expiresIn)
        {
            try
            {
                if (Contains(key))
                {
                    return Get<T>(key);
                }
                else
                {
                    var obj = fun();
                    //Set<T>(key, obj, expiresIn);
                    return obj;
                }
            }
            catch
            {

                var obj = fun();
                //Set<T>(key, obj, expiresIn);
                return obj;
            }
        }


        public int RemoveStartWith(string startStr)
        {
            var KeysCount = 0;
            var endpoints = Connection.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = Connection.GetServer(endpoint);
                var keyList = server.Keys(pattern: startStr + "*");
                KeysCount += keyList.Count();
                foreach (var item in keyList)
                {
                    //client.Remove(item);
                    // Redis=> 移除Key  
                    this.Remove(item);
                }
            }
            return KeysCount;
        }


        public List<string> GetALLKey()
        {
            //SyncRoot.EnterWriteLock();
            //var ret = client.GetAllKeys();
            //SyncRoot.ExitWriteLock();
            var endpoints = Connection.GetEndPoints(true);
            var server = Connection.GetServer(endpoints.First());
            var ret = server.Keys().Select(o => o.ToString()).ToList();
            return ret;
        }

        public void Dispose()
        {
            if (Connection != null)
            {                
                Connection.Close();
                Connection.Dispose();
            }
        }
    }
}
