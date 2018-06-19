using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Infrastructure;
using QK.QAPP.Global;
using QK.QAPP.Entity;


namespace QK.JobExcute
{
    public class JobAllCreate : IJobBase
    {
        private IScheduler _scheduler;
        private List<QB_JOB_CONFIG_INFO> _listData;

        private readonly String JOBGROUP = "QKJobGroup";// 日程组名称；
        private readonly String Trigger = "Trigger";// 日程组名称；

        public JobAllCreate(IScheduler Scheduler, List<QB_JOB_CONFIG_INFO> ListData)
        {
            _scheduler = Scheduler;
            _listData = ListData;
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        public void CreateJobs()
        {
            if (_listData != null && _listData.Count > 0)
            {
                IJobDetail jobDetail = null;
                ITrigger trigger = null;
                string jobName = "";
                foreach (QB_JOB_CONFIG_INFO model in _listData)
                {
                    jobName = "";

                    // 额度类型为空
                    if (string.IsNullOrEmpty(model.AMT_TYPE))
                    {
                        jobName = model.JOB_TYPE;
                    }
                    else
                    {
                        jobName = model.JOB_TYPE + "_" + model.AMT_TYPE;
                    }

                    // 创建job
                    jobDetail = JobBuilder.Create<QK_AutoJob>()
                        .WithIdentity(jobName, JOBGROUP)
                        .Build();

                    jobDetail.JobDataMap.Put("JobConfigModel", model);

                    if (!string.IsNullOrEmpty(model.EXCUTE_MINUTE) && !string.IsNullOrEmpty(model.EXCUTE_HOUR))
                    {
                        string cronExpression = string.Format("0 {0} {1} * * ?", Convert.ToInt32(model.EXCUTE_MINUTE).ToString(), Convert.ToInt32(model.EXCUTE_HOUR).ToString());

                        // 创建trigger
                        trigger = TriggerBuilder.Create()
                            .WithIdentity(jobName + Trigger, JOBGROUP)
                            .ForJob(jobDetail.Key)
                            .WithCronSchedule(cronExpression)
                            .Build();
                    }
                    else
                    {
                        TimeOfDay stime = new TimeOfDay(Convert.ToInt32(model.JOB_START_HOUR), Convert.ToInt32(model.JOB_START_MINUTE));
                        TimeOfDay etime = new TimeOfDay(Convert.ToInt32(model.JOB_END_HOUR), Convert.ToInt32(model.JOB_END_MINUTE));
                        int Interval = Convert.ToInt32(model.JOB_INTERAL);
                        trigger = TriggerBuilder.Create()
                           .WithIdentity(jobName + Trigger, JOBGROUP)
                           .ForJob(jobDetail.Key)
                           .WithDailyTimeIntervalSchedule(a => a.StartingDailyAt(stime).EndingDailyAt(etime).WithIntervalInMinutes(Interval))
                           .Build();
                    }

                    _scheduler.ScheduleJob(jobDetail, trigger);

                }
            }
        }
    }
}
