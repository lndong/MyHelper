using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.Extensions
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 把字符串转换为对应的枚举(忽略字符串大小写)
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="source">需要转换的字符串</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string source)
        {
            return (T) Enum.Parse(typeof(T), source, true);
        }
    }
}
