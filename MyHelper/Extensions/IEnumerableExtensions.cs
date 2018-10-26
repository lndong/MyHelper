using System;
using System.Collections.Generic;
using System.Linq;

namespace MyHelper.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 去重
        /// </summary>
        public static IEnumerable<TSource> DisTinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// 遍历集合，并对元素执行操作
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T> func)
        {
            foreach(var item in iEnumberable)
            {
                func(item);
            }
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// 遍历集合，执行方法，方法包含元素+元素索引
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumerable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumerable,Action<T,int> func)
        {
            var array = iEnumerable.ToArray();
            for(var i = 0; i<array.Count(); i++)
            {
                func(array[i], i);
            }
        }
    }
}
