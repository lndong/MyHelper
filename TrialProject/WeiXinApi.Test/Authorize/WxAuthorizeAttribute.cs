using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyHelper.WeiXin.WeiXinApi;
using WeiXinApi.Test.Helper;

namespace WeiXinApi.Test.Authorize
{
    public class WxAuthorizeAttribute: AuthorizeAttribute
    {
        /// <summary>
        /// 授权首先进入此方法，在方法中调用AuthorizeCore授权
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            filterContext.HttpContext.Response.Cache.SetNoStore();//禁止缓存，防止退出登陆后，还能后退到登录时的页面
        }

        /// <summary>
        /// 授权方法，返回true通过，false不通过
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var openId = httpContext.Session[Config.SessionOpenId];
            return openId != null;
        }

        /// <summary>
        /// 授权失败执行此方法
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var domain = Config.Domain;
            var appId = WxConfig.AppId;
            var redirectUrl = HttpUtility.UrlEncode(domain + "/WxInteractive/Callback");
            //Log4NetHelper.Info("redirectUrl:" + redirectUrl);
            var state = Guid.NewGuid().ToString().Replace("-", "");
            var url =
                $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={appId}&redirect_uri={redirectUrl}&response_type=code&scope=snsapi_base&state={state}#wechat_redirect";
            //Log4NetHelper.Info(url);
            filterContext.Result = new RedirectResult(url);
        }
    }
}