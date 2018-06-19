using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.Cache
{
    /// <summary>
    /// 缓存提供类
    /// </summary>
    public interface ICacheProvider : IDisposable
    {
        
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Set<T>(string key, T value);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        bool Set<T>(string key, T value, DateTime expiresAt);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        bool Set<T>(string key, T value, TimeSpan expiresIn);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(string key);

        /// <summary>
        /// 是否包含key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);

        /// <summary>
        /// 从缓存中获取对象，如果缓存没有，执行函数后，存入缓存后再获取
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="fun">函数</param>
        /// <returns></returns>
        T GetFromCacheOrProxy<T>(string key, Func<T> fun);
        /// <summary>
        /// 从缓存中获取对象，如果缓存没有，执行函数后，存入缓存后再获取
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="fun">函数</param>
        /// <param name="expiresAt">过期时间</param>
        /// <returns></returns>
        T GetFromCacheOrProxy<T>(string key, Func<T> fun, DateTime expiresAt);
        /// <summary>
        /// 从缓存中获取对象，如果缓存没有，执行函数后，存入缓存后再获取
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="fun">函数</param>
        /// <param name="expiresAt">过期时间</param>
        /// <returns></returns>
        T GetFromCacheOrProxy<T>(string key, Func<T> fun, TimeSpan expiresIn);

        /// <summary>
        /// 删除以 参数开头 的缓存
        /// </summary>
        /// <param name="startStr"></param>
        int RemoveStartWith(string startStr);

        /// <summary>
        /// 删除以 参数开头 的缓存
        /// </summary>
        /// <param name="startStr"></param>
        List<string> GetALLKey();
    }
}
