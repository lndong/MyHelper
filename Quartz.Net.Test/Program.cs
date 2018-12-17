using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz.Impl;

namespace Quartz.Net.Test
{
    /// <summary>
    /// Quartz.Net 2.6.2版本XMl配置试用
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new StdSchedulerFactory();
            var sc = factory.GetScheduler();
            sc.Start();
            Console.ReadKey();
        }
    }
}
