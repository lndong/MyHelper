using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXinApi.Test.Authorize;

namespace WeiXinApi.Test.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [WxAuthorize]
        public ActionResult Index()
        {
            ViewBag.Follow = "已关注";
            return View();
        }

        /// <summary>
        /// 未关注页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Failed()
        {
            ViewBag.Follow = "没有关注";
            return View();
        }
    }
}