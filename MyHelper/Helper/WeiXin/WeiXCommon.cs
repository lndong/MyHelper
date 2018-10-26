using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.Helper.WeiXin
{
    /// <summary>
    /// 微信Api公用帮助类
    /// </summary>
    public class WeiXCommon
    {
        /// <summary>
        /// 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        /// <returns>时间戳</returns>
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 生成随机串，随机串包含字母或数字
        /// </summary>
        /// <returns>随机串</returns>
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// sha1加密
        /// </summary>
        /// <param name="orgStr"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Sha1(string orgStr, string encode = "UTF-8")
        {
            var sha1 = new SHA1Managed();
            var sha1Bytes = System.Text.Encoding.GetEncoding(encode).GetBytes(orgStr);
            byte[] resultHash = sha1.ComputeHash(sha1Bytes);
            string sha1String = BitConverter.ToString(resultHash).ToLower();
            sha1String = sha1String.Replace("-", "");
            return sha1String;
        }

        /// <summary>
        /// Get提交获取
        /// </summary>
        /// <param name="sUrl"></param>
        /// <returns></returns>
        public static string WebRequestGet(string sUrl)
        {
            var client = new HttpClient();
            var result = client.GetAsync(sUrl).Result;
            return !result.IsSuccessStatusCode ? string.Empty : result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="jsapi_ticket">jsapi_ticke</param>
        /// <param name="nonceStr">随机字符串(必须与wx.config中的nonceStr相同)</param>
        /// <param name="timestamp">时间戳(必须与wx.config中的timestamp相同)</param>
        /// <param name="url">当前网页的URL，不包含#及其后面部分(必须是调用JS接口页面的完整URL)</param>
        /// <param name="resStr"></param>
        /// <returns></returns>
        public static string GetSignature(string jsapi_ticket, string timestamp, string nonceStr, string url,
            out string resStr)
        {
            string[] arrayList =
            {
                "jsapi_ticket=" + jsapi_ticket, "noncestr=" + nonceStr, "timestamp=" + timestamp,
                "url=" + url
            };
            Array.Sort(arrayList);
            var signature = string.Join("&", arrayList);
            resStr = signature;
            return Sha1(signature);
        }
    }
}
