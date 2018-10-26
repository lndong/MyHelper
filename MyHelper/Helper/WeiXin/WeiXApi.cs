using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace MyHelper.Helper.WeiXin
{
   public class WeiXApi
    {

        /// <summary>
        /// 从session中获取ticket，没有则从微信api中获取，并保存到session中
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string GetTicketSession(string appId, string appSecret)
        {
            var ticket = "";
            if (HttpContext.Current.Session["jsapi_ticket"] == null)
            {
                var token = GetTokenSession(appId, appSecret);
                ticket = string.IsNullOrEmpty(token) ? "" : GetTicket(token);
            }
            else
            {
                ticket = HttpContext.Current.Session["jsapi_ticket"].ToString();
            }
            return ticket;
        }

        /// <summary>
        /// 从session中获取token，没有则从微信api中获取，并保存到session中
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private static string GetTokenSession(string appId, string appSecret)
        {
            var tokenSession = "";
            if (HttpContext.Current.Session["SessionAccessToken"] == null)
            {
                tokenSession = GetToken(appId, appSecret);
            }
            else
            {
                tokenSession = HttpContext.Current.Session["SessionAccessToken"].ToString();
            }
            return tokenSession;
        }

       

        /// <summary>
        /// 通过微信api获取access_token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private static string GetToken(string appId, string appSecret)
        {
            var url =
                $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={appSecret}";
            var result = WeiXCommon.WebRequestGet(url);
            try
            {
                var jss = new JavaScriptSerializer();
                var resDic = (Dictionary<string, object>)jss.DeserializeObject(result);
                var token = resDic["access_token"].ToString();
                HttpContext.Current.Session["SessionAccessToken"] = token;
                HttpContext.Current.Session.Timeout = 7200;
                Log4netHelper.Info(token);
                return token;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("获取access_token失败,微信APi返回结果：" + result + ",异常信息为：" + ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
        /// 正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。
        /// 由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket 。
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string GetTicket(string token)
        {
            var url = $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={token}&type=jsapi";
            var result = WeiXCommon.WebRequestGet(url);
            try
            {
                var jss = new JavaScriptSerializer();
                var resDic = (Dictionary<string, object>)jss.DeserializeObject(result);
                var ticket = resDic["ticket"].ToString();
                HttpContext.Current.Session["jsapi_ticket"] = ticket;
                HttpContext.Current.Session.Timeout = 7200;
                Log4netHelper.Info(resDic);
                Log4netHelper.Info(ticket);
                return ticket;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("获取jsapi_ticket失败,微信APi返回结果：" + result + ",异常信息为：" + ex.Message, ex);
                return null;
            }
        }
    }
}
