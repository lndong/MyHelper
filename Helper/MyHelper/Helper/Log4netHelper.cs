using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;

namespace MyHelper.Helper
{
    public class Log4netHelper
    {
        private static ILog log;

        /// <summary>
        /// 初始化
        /// </summary>
        static Log4netHelper()
        {
            var logCfg = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            XmlConfigurator.ConfigureAndWatch(logCfg);
            log = LogManager.GetLogger("log4net");
        }

        /// <summary>
        /// 更改log4net配置中Append节点日志文件路径名(新增子目录)
        /// </summary>
        /// <param name="directoryname">新增的子目录名称</param>
        /// <param name="appendername">log4net配置文件中的Append节点名称</param>
        public static void ChangeFileName(string directoryname, string appendername)
        {
            var hierarchyLogger = log.Logger as log4net.Repository.Hierarchy.Logger;
            var appender = hierarchyLogger?.Parent.GetAppender(appendername);
            if (!(appender is RollingFileAppender rollingfileAppender)) return;
            var fileinfo = new FileInfo(rollingfileAppender.File);
            rollingfileAppender.File = Path.Combine(Path.Combine(fileinfo.DirectoryName ?? throw new InvalidOperationException(), directoryname), fileinfo.Name);
            rollingfileAppender.ActivateOptions();
        }

        /// <summary>
        /// 写调试日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Debug(string message, Exception ex)
        {
            log.Debug(message, ex);
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(string message, Exception ex)
        {
            log.Error(message, ex);
        }

        /// <summary>
        /// 写严重错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Fatal(string message, Exception ex)
        {
            log.Fatal(message, ex);
        }

        /// <summary>
        /// 写信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Info(string message, Exception ex)
        {
            log.Info(message, ex);
        }

        public static void Info(object message)
        {
            log.Info(message.ToString(), null);
        }
    }
}
