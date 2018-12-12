using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MyHelper.Helper
{
    public class TimerHelper
    {
        /// <summary>
        /// 一个简单的定时器
        /// </summary>
        /// <param name="interval">任务执行间隔时间（单位为毫秒）</param>
        /// <param name="autoReset">是否只引发一次事件(false)/true为重复引发事件</param>
        /// <param name="enabled">是否引发事件</param>
        /// <param name="elapsedEvent">事件</param>
        public static void Timer(double interval,bool autoReset,bool enabled, ElapsedEventHandler elapsedEvent)
        {
            var myTimer = new Timer
            {
                Interval = interval,
                AutoReset = autoReset,
                Enabled = enabled
            };
            myTimer.Elapsed += elapsedEvent;
        }


    }
}
