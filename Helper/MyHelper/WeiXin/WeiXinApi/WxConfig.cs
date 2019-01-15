using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.WeiXin.WeiXinApi
{
    public class WxConfig
    {
        public static readonly string AppId = ConfigurationManager.AppSettings["AppId"]; //开发者appId

        public static readonly string AppSecret = ConfigurationManager.AppSettings["AppSecret"]; //开发者密码

        public static readonly string TicketCacheKey = "TicketCacheKey"; //access_token和ticket票据缓存Key
    }
}
