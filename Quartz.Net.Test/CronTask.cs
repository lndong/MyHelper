using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Quartz.Net.Test
{
    public class CronTask : IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(CronTask));
        public void Execute(IJobExecutionContext context)
        {
            _log.Info("--- Hellow ,first CronTask---");
            //_log.Error("--- Hellow ,first CronTask---");
        }
    }
}
