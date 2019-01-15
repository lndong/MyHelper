using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace MyHelper.Custom
{
    /// <summary>
    /// 自定义MVC路由约束
    /// </summary>
    public class CustomRouteConstrain : IRouteConstraint
    {
        private readonly string _controller;

        public CustomRouteConstrain()
        {

        }

        public CustomRouteConstrain(string controller)
        {
            _controller = controller;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var param = values[parameterName]; //可以获取约束参数的值,然后在进行逻辑判断，验证通过返回true，不通过则返回false
            var controller = values["controller"]; //可以获取控制器名,对控制进行约束判断
            throw new NotImplementedException();
        }
    }
}
