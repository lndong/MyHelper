using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Quartz.Net.Test
{
    public class SimpleTask : IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(SimpleTask));
        public void Execute(IJobExecutionContext context)
        {
            var user = context.JobDetail.JobDataMap["User"];
            var msg = context.JobDetail.JobDataMap["Msg"];
            _log.Info("----" + user + "," + msg + "-----");
            //_log.Error("----" + user + "," + msg + "-----");
        }
    }
}
