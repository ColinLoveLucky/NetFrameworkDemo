using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo
{
    public sealed class TestJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TestJob));

        //public Task Execute(IJobExecutionContext context)
        //{
        //     return Task.Run(() => _logger.InfoFormat("TestJob 测试"));

        //  //  return Task.Run(() => Console.WriteLine("Hello World!"));
        //}

        void IJob.Execute(IJobExecutionContext context)
        {
            _logger.InfoFormat("TestJob 测试");
        }
    }
}
