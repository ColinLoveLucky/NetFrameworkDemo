using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace QuartzDemo
{
    public class RemotePrint: ServiceControl, ServiceSuspend
    {
        private readonly IScheduler _scheduler;
        public RemotePrint()
        {
            var properties = new NameValueCollection()
            {
                {"quartz.scheduler.instanceName", "RemoteServerSchedulerClient"},
                {"quartz.threadPool.type","Quartz.Simpl.SimpleThreadPool, Quartz" },
                { "quartz.threadPool.threadCount","5"},
                {"quartz.scheduler.exporter.type" ,"Quartz.Simpl.RemotingSchedulerExporter, Quartz"},
                { "quartz.scheduler.exporter.port","556"},
                {"quartz.scheduler.exporter.bindName","QuartzScheduler" },
                {"quartz.scheduler.exporter.channelType","tcp"}
            };
            var schedulerFactory = new StdSchedulerFactory(properties);
            var scheduler = schedulerFactory.GetScheduler();
            var job = JobBuilder.Create<HelloJob>().WithIdentity("myJob", "group1").Build();
            var trigger = TriggerBuilder.Create().WithIdentity("myJobTrigger", "group1").StartNow()
                .WithCronSchedule("/10 * * ? * *").Build();
            scheduler.ScheduleJob(job, trigger);
            _scheduler = scheduler;
            // scheduler.Start();
        }
        public bool Continue(HostControl hostControl)
        {
            _scheduler.ResumeAll();
            return true;
        }
        public bool Pause(HostControl hostControl)
        {
            _scheduler.PauseAll();
            return true;
        }
        public bool Start(HostControl hostControl)
        {
            _scheduler.Start();
            return true;
        }
        public bool Stop(HostControl hostControl)
        {
            _scheduler.Shutdown();
            return true;
        }
    }


}
