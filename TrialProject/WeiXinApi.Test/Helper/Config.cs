using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WeiXinApi.Test.Helper
{
    public class Config
    {
        public static readonly string Domain = ConfigurationManager.AppSettings["Domain"]; //域名

        public static readonly string SessionOpenId = "SessionOpenId"; //已关注公众号的微信公众号
    }
}