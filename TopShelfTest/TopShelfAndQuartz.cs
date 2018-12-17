using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace TopShelfTest
{
    /// <summary>
    /// 定时任务
    /// </summary>
   public class TopShelfAndQuartz :IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            CommonHelper.Logger.Info("第一次尝试TopShelf+Quartz构建windows服务");
        }
    }
}
