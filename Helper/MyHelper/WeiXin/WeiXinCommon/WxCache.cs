using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHelper.Caches;
using MyHelper.Helper;
using Newtonsoft.Json;

namespace MyHelper.WeiXin.WeiXinCommon
{
    /// <summary>
    /// 缓存token帮助类
    /// </summary>
    public class WxCache
    {
        private static readonly string Folder = "Token";

        private static readonly ICache Cache;

        static WxCache()
        {
            Cache = new HttpCache();
        }

        /// <summary>
        /// 保存票据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="token"></param>
        /// <param name="timeSpan"></param>
        public static void SetToken(string cacheKey, WxTicketCacheModel token, TimeSpan timeSpan)
        {
            //先缓存到内存中
            Cache.SetCache(cacheKey, token, timeSpan, ExpireType.Absolute);
            //内存中可能因服务器内存原因提前被销毁，在存一份到本地文件中
            var content = JsonConvert.SerializeObject(token);
            FileHelper.WriteToken(content, Folder, cacheKey);
        }

        /// <summary>
        /// 把对象属性值组成字符串
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string GetContent(WxTicketCacheModel token)
        {
            var sb = new StringBuilder();
            sb.Append(token.AccessToken).Append(";").Append(token.JsApiTicket).Append(";")
                .Append(token.ExpireTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return sb.ToString();
        }

        /// <summary>
        /// 获取票据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static WxTicketCacheModel GetToken(string cacheKey)
        {
            //首先从内存中读取
            var obj = Cache.GetCache(cacheKey);
            if (obj != null)
            {
                return obj as WxTicketCacheModel;
            }

            #region 当内存中不存在时从文本记录中获取ticket

            //如果内存中不存在，则从文本中读取
            var fileStr = FileHelper.GetCacheByFile(Folder, cacheKey);
            if (string.IsNullOrEmpty(fileStr)) return null;
            var ticketObj = JsonConvert.DeserializeObject<WxTicketCacheModel>(fileStr);
            //如果过期时间大于当前时间加3分钟则从文本中获取token，并保存到内存中
            if (ticketObj.ExpireTime <= DateTime.Now.AddMinutes(3))
            {
                FileHelper.DeleteFile(Folder, cacheKey);//过期则删除文件
                return null;
            }
            var timeSpan = ticketObj.ExpireTime - DateTime.Now;
            Log4netHelper.Info("从文本中获取token并缓存到内存中");
            Cache.SetCache(cacheKey, ticketObj, timeSpan, ExpireType.Absolute); //没过期，重新存到内存中
            return ticketObj;

            #endregion

        }

        /// <summary>
        /// 缓存中是否还有token
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static bool ContainsKey(string cacheKey)
        {
            return GetToken(cacheKey) != null;
        }
    }


    /// <summary>
    /// 凭据缓存对象
    /// </summary>
    public class WxTicketCacheModel
    {
        public string AccessToken { get; set; }

        public string JsApiTicket { get; set; }

        public DateTime ExpireTime { get; set; }
    }
}
