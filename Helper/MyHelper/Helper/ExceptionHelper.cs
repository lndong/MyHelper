using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.Helper
{
    /// <summary>
    /// catch异常输出日志帮助类
    /// </summary>
    public class ExceptionHelper
    {
        /// <summary>
        /// 记录异常日志并返回false
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool WriteExLogRBool(string msg, Exception ex)
        {
            return WriteLogReturn(msg, ex, false);
        }

        /// <summary>
        /// 记录异常日志无返回
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void WriteExlogVoid(string msg, Exception ex)
        {
            Log4netHelper.Error(msg + ex.Message, ex);
        }

        /// <summary>
        /// 记录异常日志，并返回
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="msg">日志内容</param>
        /// <param name="ex">异常</param>
        /// <param name="t">返回值</param>
        /// <returns></returns>
        public static T WriteLogReturn<T>(string msg, Exception ex,T t)
        {
            WriteExlogVoid(msg, ex);
            return t;
        }
    }
}
