using System;
using MyHelper.WeiXin.WeiXinCommon;

namespace MyHelper.WeiXin.WeiXinApi
{
    /// <summary>
    /// 获取票据并缓存
    /// </summary>
    public class WxTicket
    {
        private string _accessToken;

        private string _jsApiTicket;

        /// <summary>
        /// access_token
        /// </summary>
        public string AccessToken
        {
            get
            {
                _accessToken = GetAccessToken(WxConfig.AppId, WxConfig.AppSecret);
                return _accessToken;
            }
        }

        /// <summary>
        /// JS-SDK js_ticket
        /// </summary>
        public string JsApiTicket
        {
            get
            {
                _jsApiTicket = GetJsApiTicket(WxConfig.AppId, WxConfig.AppSecret);
                return _jsApiTicket;
            }
        }

        /// <summary>
        /// 获取微信access_token
        /// 在不需要Jsapi_ticket时使用
        /// </summary>
        /// <param name="appId">公众号appId</param>
        /// <param name="appSecret">公众号开发者密码</param>
        /// <returns></returns>
        private string GetAccessToken(string appId, string appSecret)
        {
            string token;
            string ticket;
            if (WxCache.ContainsKey(WxConfig.TicketCacheKey))
            {
                var obj = WxCache.GetToken(WxConfig.TicketCacheKey);
                token = obj.AccessToken;
                ticket = obj.JsApiTicket;
            }
            else
            {
                token = WxBaseApi.GetAccessToken(appId, appSecret);
                if (string.IsNullOrEmpty(token)) return null;
                ticket = JspApi.GetTicket(token);
                SetCache(token, ticket);
            }
            return token;
        }

        /// <summary>
        /// 获取jsapi_ticket，并缓存
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private string GetJsApiTicket(string appId, string appSecret)
        {
            string token;
            var ticket = "";
            if (WxCache.ContainsKey(WxConfig.TicketCacheKey))
            {
                var obj = WxCache.GetToken(WxConfig.TicketCacheKey);
                token = obj.AccessToken;
                ticket = obj.JsApiTicket;
            }

            if (!string.IsNullOrEmpty(ticket)) return ticket;
            token = WxBaseApi.GetAccessToken(appId, appSecret);
            if (string.IsNullOrEmpty(token)) return null;
            ticket = JspApi.GetTicket(token);
            if (string.IsNullOrEmpty(ticket)) return null;
            SetCache(token, ticket);
            return ticket;
        }

        /// <summary>
        /// 缓存access_token和jsApi_ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="jsApiTicket"></param>
        private void SetCache(string accessToken, string jsApiTicket)
        {
            var timespan = new TimeSpan(2, 0, 0); //缓存时间差
            var expireTime = DateTime.Now.AddTicks(timespan.Ticks);
            var model = new WxTicketCacheModel
            {
                AccessToken = accessToken,
                JsApiTicket = jsApiTicket,
                ExpireTime = expireTime
            };
            WxCache.SetToken(WxConfig.TicketCacheKey, model, timespan);
        }
    }
}
