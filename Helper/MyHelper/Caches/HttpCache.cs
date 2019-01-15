using System;
using System.Web;
using System.Web.Caching;

namespace MyHelper.Caches
{
    /// <summary>
    /// Http缓存（HttpRuntime.Cache）
    /// </summary>
    public class HttpCache : ICache
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        public void SetCache(string key, object value)
        {
            _SetCache(key, value, null, null);
        }

        /// <summary>
        /// 设置缓存
        /// 注：默认过期类型为绝对过期
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间间隔</param>
        public void SetCache(string key, object value, TimeSpan timeout)
        {
            _SetCache(key, value, timeout, ExpireType.Absolute);
        }

        /// <summary>
        /// 设置缓存
        /// 注：默认过期类型为绝对过期
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间间隔</param>
        /// <param name="expireType">过期类型</param>
        public void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType)
        {
            _SetCache(key, value, timeout, expireType);
        }

        /// <summary>
        /// 设置键失效时间
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="expire">从现在起时间间隔</param>
        public void SetKeyExpire(string key, TimeSpan expire)
        {
            var value = GetCache(key);
            SetCache(key, value, expire);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">主键</param>
        public object GetCache(string key)
        {
            return HttpRuntime.Cache[key];
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <typeparam name="T">数据类型</typeparam>
        public T GetCache<T>(string key) where T : class
        {
            return (T) HttpRuntime.Cache[key];
        }

        /// <summary>
        /// 是否存在键值
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return GetCache(key) != null;
        }


        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">主键</param>
        public void RemoveCache(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间间隔</param>
        /// <param name="exception">过期类型</param>
        private void _SetCache(string key, object value, TimeSpan? timeout, ExpireType? exception)
        {
            if (timeout == null)
                HttpRuntime.Cache[key] = value;
            else
            {
                if (exception == ExpireType.Absolute) //绝对到期
                {
                    var endTime = DateTime.Now.AddTicks(timeout.Value.Ticks);
                    HttpRuntime.Cache.Insert(key, value, null, endTime, Cache.NoSlidingExpiration);
                }
                else
                {
                    HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, timeout.Value);
                }
            }
        }
    }
}
