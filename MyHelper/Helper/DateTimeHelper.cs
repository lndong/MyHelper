using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.Helper
{
    /// <summary>
    /// DateTime 帮助类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// 返回默认时间1970-01-01
        /// </summary>
        /// <returns></returns>
        public static DateTime DefaultTime()
        {
            return DateTime.Parse("1970-01-01");
        }

        /// <summary>
        /// 时间转时间戳(1970.1.1)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToInt(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000;
        }

        /// <summary>
        /// 时间戳转为时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime IntToDateTime(long timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(timestamp);
        }

        /// <summary>
        /// 获取某一日期是该年中的第几周
        /// </summary>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime dateTime)
        {
            var gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// 计算两个时间之间的间隔返回整毫秒数
        /// </summary>
        /// <param name="oneTime">前一个时间</param>
        /// <param name="otherTime">后一个时间</param>
        /// <returns></returns>
        public static double GetInervalTime(DateTime oneTime, DateTime otherTime)
        {
            var timeSpan = otherTime.Subtract(oneTime);
            return timeSpan.TotalMilliseconds;
        }
    }
}
