using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MyHelper.Helper;
using MyHelper.WeiXin.WeiXinApi;
using MyHelper.WeiXin.WeiXinCommon;
using WeiXinApi.Test.Models;

namespace WeiXinApi.Test.Helper
{
    public class WxHelper
    {
        /// <summary>
        /// 判断用户是否关注公众号
        /// </summary>
        /// <param name="code"></param>
        /// <param name="openId">返回用户openId</param>
        /// <returns></returns>
        public bool IsFollow(string code, out string openId)
        {
            string access;

            //获取openId
            openId = WxBaseApi.GetOpenIdByCode(code, WxConfig.AppId, WxConfig.AppSecret, out access);
            if (string.IsNullOrEmpty(openId)) return false;
            //获取access_token
            var wxTicket = new WxTicket();
            var token = wxTicket.AccessToken;
            if (string.IsNullOrEmpty(token)) return false;
            //获取用户信息，subscribe = 1时表示关注，0为没有关注
            var userObj = WxBaseApi.GetUserInfo(openId, token);
            var subscribe = userObj.SelectToken("subscribe");
            if (subscribe == null) return false;
            return subscribe.ToString() == "1";
        }

        /// <summary>
        /// 获取JS-SDK页面config配置信息model
        /// </summary>
        /// <param name="url">当前请求页面url</param>
        /// <returns></returns>
        public JsSdkModel GetJsSdkModel(string url)
        {
            var wx = new WxTicket();
            var jsapi_ticket = wx.JsApiTicket;
            var timestamp = Util.GenerateTimestamp();
            var nonceStr = Util.GenerateNonceStr();
            string strs;
            var signature = JspApi.GetSignature(jsapi_ticket, timestamp.ToString(), nonceStr, url, out strs);
            var model = new JsSdkModel
            {
                AppId = WxConfig.AppId,
                Timestamp = timestamp,
                NonceStr = nonceStr,
                JsApiTicket = jsapi_ticket,
                Signature = signature,
                Strs = strs
            };
            return model;
        }

        /// <summary>
        /// 把从微信服务器下载下来的图片流保存到本地
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public async Task<bool> DownloadImageFromWeiXin(string mediaId)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Token\\" + DateTime.Now.Ticks + ".jpg";
            Log4netHelper.Info(path);
            var wx = new WxTicket();
            var token = wx.AccessToken;
            try
            {
                var stream = await WxMediaApi.DownLoadFileTask(token, mediaId);
                var bitmap = new Bitmap(stream);
                bitmap.Save(path);
                return true;
            }
            catch (Exception ex)
            {
                var msg = "流存储为图片时出错：";
                return ExceptionHelper.WriteExLogRBool(msg, ex);
            }


        }
    }
}