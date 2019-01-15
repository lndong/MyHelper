using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXinApi.Test.Models
{
    /// <summary>
    /// JS-SDK页面config配置信息模型
    /// </summary>
    public class JsSdkModel
    {

        public string AppId { get; set; }

        public long Timestamp { get; set; }

        public string NonceStr { get; set; }

        public string JsApiTicket { get; set; }

        /// <summary>
        /// 签名后字符串
        /// </summary>
        public string Signature { get; set; }


        /// <summary>
        /// 签名未使用sha1加密的字符串
        /// </summary>
        public string Strs { get; set; }
        //public string ShareUrl { get; set; }

        //public string ShareImg { get; set; }
    }
}