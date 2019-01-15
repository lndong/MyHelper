using System;
using System.Runtime.Caching;

namespace MyHelper.Caches
{
    /// <summary>
    /// 系统缓存 MemoryCache
    /// </summary>
    public class SystemCache:ICache
    {
        private readonly MemoryCache _cache;

        public SystemCache()
        {
            _cache = MemoryCache.Default;
        }

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
            _SetCache(key,value,timeout,ExpireType.Absolute);
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
            SetCache(key,value,expire);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">主键</param>
        public object GetCache(string key)
        {
            return _cache[key];
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <typeparam name="T">数据类型</typeparam>
        public T GetCache<T>(string key) where T : class
        {
            return (T) GetCache(key);
        }

        /// <summary>
        /// 是否存在键值
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _cache.Contains(key);
        }


        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">主键</param>
        public void RemoveCache(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间间隔</param>
        /// <param name="exception">过期类型</param>
        /// MemoryCache还支持文件依赖过期(文件有变动就过期重新加载)
        private void _SetCache(string key, object value, TimeSpan? timeout, ExpireType? exception)
        {
            var policy = new CacheItemPolicy();
            if (timeout == null)
            {
                policy.Priority = CacheItemPriority.NotRemovable; //优先级为不删除
            }
            else
            {
                if (exception != ExpireType.Relative) //不为滑动过期时
                {
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddTicks(timeout.Value.Ticks);
                }
                else
                {
                    policy.SlidingExpiration = timeout.Value;
                }
            }

            _cache.Set(key, value, policy);

        }
    }
}
