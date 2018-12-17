using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace TopShelfTest
{
    public class CommonHelper
    {
        public static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取当前进程Id。
        /// </summary>
        /// <returns></returns>
        public static int GetProcessId()
        {
            return System.Diagnostics.Process.GetCurrentProcess().Id;
        }


        /// <summary>
        /// 获取当前线程Id。
        /// </summary>
        /// <returns></returns>
        public static int GetThreadId()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId;
        }
    }

}
