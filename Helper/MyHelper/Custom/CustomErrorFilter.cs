using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyHelper.Helper;

namespace MyHelper.Custom
{
    /// <summary>
    /// 自定义返回JSON对象的异常处理Filter
    /// 使用此Filter必须把web.config中的customError的mode设为On或者RemoteOnly才能生效
    /// 可以使用在controller和action上面，减少try，catch
    /// </summary>
    public class CustomErrorFilter : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 发生异常时调用
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            //获取出现异常的控制器和action
            var controller = filterContext.RouteData.Values["controller"]; //控制器
            var action = filterContext.RouteData.Values["action"]; //方法
            var exception = filterContext.Exception; //异常
            var errorMessage = $"调用{controller}控制{action}方法时发生异常，异常信息为：" + exception.Message;
            Log4netHelper.Error(errorMessage, exception);
            //返回json对象
            filterContext.Result = new JsonResult
            {
                Data = new {erorMessage = exception.Message},
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
            filterContext.ExceptionHandled = true; //此处设置为true表明异常使用此类处理完成
        }
    }
}
