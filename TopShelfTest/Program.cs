using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace TopShelfTest
{
    /// <summary>
    /// 使用TopShelf构建windows服务，使用Quartz 2.6.2配置定时任务
    /// 使用TopShelf构建服务便于调试，直接F5断点调试
    /// 安装也方便，使用windows自带cmd进行方法
    /// eg：安装服务: D:\EXER\VS2017\Git\TopShelfTest\bin\Debug\TopShelfTest.exe install
    /// 启动服务: D:\EXER\VS2017\Git\TopShelfTest\bin\Debug\TopShelfTest.exe start
    /// 停止服务：D:\EXER\VS2017\Git\TopShelfTest\bin\Debug\TopShelfTest.exe stop
    /// 卸载服务：D:\EXER\VS2017\Git\TopShelfTest\bin\Debug\TopShelfTest.exe uninstall
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            CommonHelper.Logger.InfoFormat("正在准备安装日志服务1，PID：{0}，TID：{1}", CommonHelper.GetProcessId(),
                CommonHelper.GetThreadId());
            //HostFactory.Run() 等价于HostFactory.New().Run()

            #region 方式一

            //GetHostNoTopShelf().Run();

            #endregion

            #region 方式二

            HostRun();

            #endregion

        }

        /// <summary>
        /// 不继承TopShelf构建服务
        /// </summary>
        /// <returns></returns>
        private static Host GetHostNoTopShelf()
        {
            var host = HostFactory.New(o =>
            {
                o.Service<FirstService>(f =>
                {
                    f.ConstructUsing(x => new FirstService()); //构建实例
                    f.WhenStarted(x => x.Start()); //开始运行执行方法
                    f.WhenStopped(x => x.Stop()); //停止时执行方法
                });
                o.RunAsLocalSystem(); //使用本子系统账号来运行
                o.SetServiceName("FirstTestService"); //服务名称
                o.SetDisplayName("TopShelf日志服务"); //显示名称
                o.SetDescription("自定义服务类"); //服务描述
            });
            return host;
        }

        /// <summary>
        /// 继承TopShelf构建服务
        /// </summary>
        private static void HostRun()
        {
            HostFactory.Run(o =>
            {
                o.Service<InheritTopShelf>();
                o.RunAsLocalSystem(); //使用本子系统账号来运行
                o.SetServiceName("SecondTestService"); //服务名称
                o.SetDisplayName("TopShelfSecond日志服务"); //显示名称
                o.SetDescription("自定义服务类"); //服务描述
            });
        }
    }
}
