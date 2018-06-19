using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo
{
    public class UsingQuartz
    {
        public async void Test()
        {
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type","binary"}
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            IScheduler sched =  factory.GetScheduler();
             sched.Start();
            IJobDetail job = JobBuilder.Create<HelloJob>().WithIdentity("myJob", "group1").Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("myTrigger", "group1").
                StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(40).RepeatForever()).Build();
             sched.ScheduleJob(job, trigger);
        }

        public async void DumpJob()
        {
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type","binary"}
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            IScheduler sched =  factory.GetScheduler();
             sched.Start();
            IJobDetail job = JobBuilder.Create<DumpJob>().WithIdentity("myJob", "group1")
                .UsingJobData("jobSays", "Hello World!").
                UsingJobData("myFloatValue", 3.14f)
                .Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("myTrigger", "group1").
                StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(40).RepeatForever()).Build();
             sched.ScheduleJob(job, trigger);
        }
    }
}
