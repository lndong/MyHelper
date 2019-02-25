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

        #region base64加解密

        /// <summary>
        /// Base64加密(字符编码为UTF8)
        /// </summary>
        /// <param name="source">需加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(this string source)
        {
            return source.Base64Encode(Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">需加密字符串</param>
        /// <param name="encoding">加密采用的字符编码（默认为UTF8）</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(this string source, Encoding encoding)
        {
            if (string.IsNullOrEmpty(source)) return source;
            var bytes = encoding.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解密(默认采用UTF8编码)
        /// </summary>
        /// <param name="source">待解密字符串</param>
        /// <returns>解密后字符串</returns>
        public static string Base64Decode(this string source)
        {
            return source.Base64Decode(Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="source">待解密字符串</param>
        /// <param name="encoding">解密采用的字符编码</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(this string source, Encoding encoding)
        {
            if (string.IsNullOrEmpty(source)) return source;
            var bytes = Convert.FromBase64String(source);
            return encoding.GetString(bytes);
        }

        #endregion
    }

}
