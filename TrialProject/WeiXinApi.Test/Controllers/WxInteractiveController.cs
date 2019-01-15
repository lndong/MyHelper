using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyHelper.Helper;
using WeiXinApi.Test.Helper;

namespace WeiXinApi.Test.Controllers
{
    public class WxInteractiveController : Controller
    {
        // GET: WxInteractive
        /// <summary>
        /// 微信回调
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Callback()
        {
            var code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("Failed", "Home");
            }
            Log4netHelper.Info("code值为：" + code);
            var wx = new WxHelper();
            string openId;
            if (!wx.IsFollow(code, out openId)) return RedirectToAction("Failed", "Home");
            Session[Config.SessionOpenId] = openId;
            Session.Timeout = 1;
            return RedirectToAction("Index", "Home");
        }
    }
}