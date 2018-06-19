using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo
{
    public class FirstQuartz
    {
        public void Test()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            IJobDetail job1 = JobBuilder.Create<HelloJob>().WithIdentity("T1", "TG1").Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("Tr1", "TrG1").StartNow().
                            WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()).Build();
            scheduler.ScheduleJob(job1, trigger);
        }
    }
}
