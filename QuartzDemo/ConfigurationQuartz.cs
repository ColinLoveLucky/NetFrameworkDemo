using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace QuartzDemo
{
    public class ConfigurationQuartz
    {
        public void XmlConfiguration()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();
            sched.Start();
        }
    }
}
