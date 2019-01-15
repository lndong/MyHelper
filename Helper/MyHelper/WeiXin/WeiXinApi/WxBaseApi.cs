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
    public class WxBaseApi
    {
        /// <summary>
        /// 通过微信api获取access_token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string appId, string appSecret)
        {
            var url =
                $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={appSecret}";
            var result = Util.RequestWxApi(url);
            try
            {
                var obj = JObject.Parse(result);
                Log4netHelper.Info("tokenObj:" + result);
                var token = obj.SelectToken("access_token")?.ToString();
                return token;
            }
            catch (Exception ex)
            {
                var msg = "调用微信api获取access_token异常：" + ex.Message;
                return ExceptionHelper.WriteExLogRNull(msg, ex)?.ToString();
            }
        }

        /// <summary>
        /// 获取openId（不管用户是否关注公众号都可以获取）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="accessToken">网页授权access_token,与接口调用凭证（GetAccessToken方法获取的accessToken）不同</param>
        /// <returns></returns>
        public static string GetOpenIdByCode(string code, string appId, string appSecret, out string accessToken)
        {
            var url =
                $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appId}&secret={appSecret}&grant_type=authorization_code&code={code}";
            var result = Util.RequestWxApi(url);
            try
            {
                string openId;
                if (!string.IsNullOrEmpty(result))
                {
                    var obj = JObject.Parse(result);
                    openId = obj.SelectToken("openid").ToString();
                    accessToken = obj.SelectToken("access_token")?.ToString();
                }
                else
                {
                    accessToken = "";
                    openId = "";
                }
                return openId;
            }
            catch (Exception ex)
            {
                Log4netHelper.Info("微信接口返回结果：" + result);
                var msg = "获取openId发生异常：" + ex.Message;            
                accessToken = "";
                return ExceptionHelper.WriteExLogRNull(msg, ex)?.ToString();
            }
        }

        /// <summary>
        /// 根据openId和accesstoken获取用户基本信息
        /// </summary>
        /// <param name="openId">用户openId</param>
        /// <param name="accessToken">接口凭证</param>
        /// <returns></returns>
        public static JObject GetUserInfo(string openId, string accessToken)
        {
            var url =
                $"https://api.weixin.qq.com/cgi-bin/user/info?access_token={accessToken}&openid={openId}&lang=zh_CN";
            var result = Util.RequestWxApi(url);
            if (string.IsNullOrEmpty(result)) return null;
            Log4netHelper.Info("微信api接口返回用户信息：" + result);
            try
            {
                return JObject.Parse(result);
            }
            catch (Exception ex)
            {
                var msg = "获取用户信息发生异常：" + ex.Message;
                ExceptionHelper.WriteExlogVoid(msg, ex);
                return null;
            }
        }
    }
}
