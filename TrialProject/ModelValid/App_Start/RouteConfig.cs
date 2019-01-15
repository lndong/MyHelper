using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ModelValid
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region 伪静态路由

            routes.MapRoute(
                "Action1Html", // action伪静态  
                "{controller}/{action}.html", // 没有参数的 URL  
                new { controller = "User", action = "Register", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "IDHtml", // id伪静态  
                "{controller}/{action}/{id}.html", // 带有参数的 URL  
                new { controller = "User", action = "Register", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ActionHtml", // action伪静态  
                "{controller}/{action}.html/{id}", // 带有参数的 URL  
                new { controller = "User", action = "Register", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ControllerHtml", // controller伪静态  
                "{controller}.html/{action}/{id}",// 带有参数的 URL  
                new { controller = "User", action = "Register", id = UrlParameter.Optional }
            );

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional }
            );
        }
    }
}
