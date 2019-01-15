using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MyHelper.Helper;

namespace MyHelper.WeiXin.WeiXinCommon
{
    /// <summary>
    /// 微信公用方法
    /// </summary>
    public class Util
    { /// <summary>
        /// 调用微信API接口
        /// </summary>
        /// <param name="url">接口url地址</param>
        /// <returns></returns>
        public static string RequestWxApi(string url)
        {
            try
            {
                var client = new HttpClient();
                var result = client.GetAsync(url).Result;
                return result.IsSuccessStatusCode ? result.Content.ReadAsStringAsync().Result : string.Empty;
            }
            catch (Exception e)
            {
                var msg = "访问微信api异常，异常url为：" + url + ",message:" + e.Message;
                ExceptionHelper.WriteExlogVoid(msg,e);
                return string.Empty;
            }
        }

        /// <summary>
        /// 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        /// <returns></returns>
        public static long GenerateTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMinutes);
        }

        /// <summary>
        /// 生成随机串，随机串包含字母或数字
        /// </summary>
        /// <returns></returns>
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// sha1加密
        /// </summary>
        /// <param name="orgStr">需要加密字符串</param>
        /// <param name="encode">编码(默认UTF-8)</param>
        /// <returns></returns>
        public static string Sha1(string orgStr, string encode = "UTF-8")
        {
            var encoding = Encoding.GetEncoding(encode);
            return EncryptHelper.Sha1Encrypt(orgStr, encoding);
        }
    }
}
