using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.Helper
{
    public class StringHelper
    {
        #region Base64加密

        /// <summary>
        /// Base64加密(字符编码为UTF8)
        /// </summary>
        /// <param name="source">需加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(source, Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">需加密字符串</param>
        /// <param name="encodeType">加密采用的字符编码（默认为UTF8）</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source, Encoding encodeType)
        {
            if (string.IsNullOrEmpty(source)) return source;
            var bytes = encodeType.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }

        #endregion


        #region Base64解密

        /// <summary>
        /// Base64解密(默认采用UTF8编码)
        /// </summary>
        /// <param name="source">待解密字符串</param>
        /// <returns>解密后字符串</returns>
        public static string Base64Decode(string source)
        {
            return Base64Decode(source, Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="source">待解密字符串</param>
        /// <param name="encodeType">解密采用的字符编码</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string source, Encoding encodeType)
        {
            if (string.IsNullOrEmpty(source)) return source;
            var bytes = Convert.FromBase64String(source);
            return encodeType.GetString(bytes);
        }

        #endregion
    }
}
