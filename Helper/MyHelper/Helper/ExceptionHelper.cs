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
            return (bool)WriteExlogRObject(msg, ex, false);
        }

        /// <summary>
        /// 记录异常日志并返回Null
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static object WriteExLogRNull(string msg, Exception ex)
        {
            return  WriteExlogRObject(msg, ex, null);
        }

        /// <summary>
        /// 记录异常日志并返回对象
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object WriteExlogRObject(string msg, Exception ex, object obj)
        {
            WriteExlogVoid(msg, ex);
            return obj;
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
    }
}
