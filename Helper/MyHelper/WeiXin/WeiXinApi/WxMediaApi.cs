using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyHelper.Helper;

namespace MyHelper.WeiXin.WeiXinApi
{
    /// <summary>
    /// 素材管理API
    /// </summary>
    public class WxMediaApi
    {
        /// <summary>
        /// 从临时素材服务器下载图片文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId"></param>
        /// <returns>返回图片文件二进制流</returns>
        public static async Task<Stream> DownLoadFileTask(string accessToken, string mediaId)
        {
            var url = $"https://api.weixin.qq.com/cgi-bin/media/get?access_token={accessToken}&media_id={mediaId}";
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;
                var stream = await response.Content.ReadAsStreamAsync();
                return stream;
            }
            catch (Exception ex)
            {
                var msg = "调用下载临时素材微信api接口发生异常：";
                return (Stream)ExceptionHelper.WriteExLogRNull(msg, ex);
            }
        }
    }
}
