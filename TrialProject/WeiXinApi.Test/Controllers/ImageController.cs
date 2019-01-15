using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyHelper.Helper;
using Newtonsoft.Json;
using WeiXinApi.Test.Helper;

namespace WeiXinApi.Test.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            var wx = new WxHelper();
            var url = Request.Url?.ToString();
            var model = wx.GetJsSdkModel(url);
            Log4netHelper.Info(JsonConvert.SerializeObject(model));
            return View(model);
        }

        /// <summary>
        /// 通过微信serverId把图片保存到本地服务器
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> SaveInfo()
        {
            var mediaIds = Request.QueryString["mediaIds"]; //获取微信serviceId集合字符串
            Log4netHelper.Info(mediaIds);
            var b = true;
            if (string.IsNullOrEmpty(mediaIds))
            {
                b = false;
            }
            else
            {
                var wx = new WxHelper();
                var mediaIdStrs = mediaIds.Split(';');
                foreach (var mediaId in mediaIdStrs)
                {
                    Log4netHelper.Info("图片serviceId：" + mediaId);
                    if (await wx.DownloadImageFromWeiXin(mediaId)) continue;
                    b = false;
                    break;
                }
            }
            return Json(new { success = b }, JsonRequestBehavior.AllowGet);
        }
    }
}