using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Xml;
using Topshelf;

namespace TopShelfTest
{
    /// <summary>
    /// 继承TopShelf构建服务
    /// </summary>
    public class InheritTopShelf : ServiceControl, ServiceSuspend, ServiceShutdown
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(InheritTopShelf));
        private static IScheduler _scheduler;

        public InheritTopShelf()
        {
            _log.Info("构建");
            //手动加载配置文件
            XMLSchedulingDataProcessor processor = new XMLSchedulingDataProcessor(new SimpleTypeLoadHelper());
            var factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler();
            var path = AppDomain.CurrentDomain.BaseDirectory + "quartz_jobs.xml";
            processor.ProcessFileAndScheduleJobs(path,_scheduler);
        }

        /// <summary>
        /// 服务开始
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Start(HostControl hostControl)
        {
            _log.Info("服务开始");
            //var factory = new StdSchedulerFactory();
            //_scheduler = factory.GetScheduler();
            _scheduler.Start();
            _log.Info("任务启动");
            return true;
        }

        /// <summary>
        /// 服务停止
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Stop(HostControl hostControl)
        {
            _scheduler.Shutdown(false); //true:等待正在运行的计划任务执行完毕在关闭；false：强制关闭 
            _log.Info("服务停止");
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            throw new NotImplementedException();
        }

        public bool Continue(HostControl hostControl)
        {
            throw new NotImplementedException();
        }

        public void Shutdown(HostControl hostControl)
        {
            throw new NotImplementedException();
        }
    }
}
