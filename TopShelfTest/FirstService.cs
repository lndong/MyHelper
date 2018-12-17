using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace TopShelfTest
{
    /// <summary>
    /// 第一种方法
    /// </summary>
    public class FirstService
    {
        private static IScheduler _scheduler;

        public FirstService()
        {
            #region 调试成功部署异常
            //在此处构建调度器时调试正常，但部署会报异常，File named 'quartz_jobs.xml' does not exist
            //具体原因没去研究
            //var factory = new StdSchedulerFactory();
            //_scheduler = factory.GetScheduler(); 
            //InheritTopShelf方法中手动加载配置文件可以运行正常

            #endregion

            CommonHelper.Logger.Info("构建服务对象");
        }

        #region 必须方法

        /// <summary>
        /// 服务启动执行方法
        /// </summary>
        public void Start()
        {
            //...逻辑代码
            var factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler();
            _scheduler.Start();
            CommonHelper.Logger.InfoFormat("服务已开启，PID：{0}，TID：{1}，调用 Start() 方法", CommonHelper.GetProcessId(),
                CommonHelper.GetThreadId());
        }

        /// <summary>
        /// 服务停止执行方法
        /// </summary>
        public void Stop()
        {
            _scheduler.Shutdown();
            //...逻辑代码
            CommonHelper.Logger.InfoFormat("服务已停止，PID：{0}，TID：{1}，调用 Stop() 方法", CommonHelper.GetProcessId(),
                CommonHelper.GetThreadId());
        }

        #endregion


        #region 可选方法

        /// <summary>
        /// 服务暂停
        /// </summary>
        public void Pause()
        {
            //...逻辑代码
            CommonHelper.Logger.InfoFormat("服务已暂停，PID：{0}，TID：{1}，调用 Pause() 方法", CommonHelper.GetProcessId(),
                CommonHelper.GetThreadId());
        }

        /// <summary>
        ///服务继续
        /// </summary>
        public void Continue()
        {
            //...逻辑代码
            CommonHelper.Logger.InfoFormat("服务继续运行，PID：{0}，TID：{1}，调用 Continue() 方法", CommonHelper.GetProcessId(),
                CommonHelper.GetThreadId());
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Shutdown()
        {
            //...逻辑代码
            CommonHelper.Logger.InfoFormat("服务已关闭，PID：{0}，TID：{1}，调用 Shutdown() 方法", CommonHelper.GetProcessId(),
                CommonHelper.GetThreadId());
        }
        #endregion
    }
}
