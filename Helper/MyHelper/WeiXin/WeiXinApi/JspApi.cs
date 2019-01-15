using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHelper.Helper;
using MyHelper.WeiXin.WeiXinCommon;
using Newtonsoft.Json.Linq;

namespace MyHelper.WeiXin.WeiXinApi
{
    /// <summary>
    /// 微信JS-SDK
    /// </summary>
    public class JspApi
    {

        /// <summary>
        /// 获取jssdk票据
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetTicket(string access_token)
        {
            var url = $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={access_token}&type=jsapi";
            var requestResult = Util.RequestWxApi(url);
            try
            {
                if (string.IsNullOrEmpty(requestResult)) return null;
                var jsObj = JObject.Parse(requestResult);
                Log4netHelper.Info("jsapi_ticket对象：" + jsObj);
                var ticket = jsObj.SelectToken("ticket")?.ToString();
                return ticket;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("获取jsapi_ticket异常：" + ex.Message, ex);
                return null;
            }

        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="jsapi_ticket">Jsapi票据</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonceStr">随机字符串</param>
        /// <param name="url">当前网页的url</param>
        /// <returns></returns>
        public static string GetSignature(string jsapi_ticket, string timestamp, string nonceStr, string url, out string str)
        {
            var sb = new StringBuilder();
            sb.Append("jsapi_ticket=").Append(jsapi_ticket).Append("&").Append("noncestr=").Append(nonceStr).Append("&")
                .Append("timestamp=").Append(timestamp).Append("&").Append("url=").Append(url);
            str = sb.ToString();
            return Util.Sha1(str);
        }
    }
}
