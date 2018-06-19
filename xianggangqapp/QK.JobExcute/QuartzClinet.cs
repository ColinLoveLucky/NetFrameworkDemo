using QK.QAPP.Entity;
using QK.QAPP.Global;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace QK.JobExcute
{
    /// <summary>
    /// 任务调度通用类
    /// </summary>
    public class QuartzClinet
    {
        private static IScheduler _scheduler;
        private static String JOBGROUP = "QKJobGroup";// 日程组名称；
        private static String Trigger = "Trigger";// 日程组名称；

        public static IScheduler Scheduler
        {
            get
            {
                if (_scheduler == null)
                {
                    InitSchedule();
                }
                return _scheduler;
            }
        }

        static QuartzClinet()
        {
            try
            {
                if (_scheduler == null)
                {
                    InitSchedule();
                }
            }
            catch (SchedulerException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void InitSchedule()
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteClient";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "10";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // set remoting exporter
            properties["quartz.scheduler.proxy"] = "true";
            properties["quartz.scheduler.proxy.address"] = GlobalSetting.ServerName;

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory(properties);
            _scheduler = schedulerFactory.GetScheduler();// 生成日程对象；
        }

        public static bool CheckJobExist(QB_JOB_CONFIG_INFO model)
        {
            string JobName = GetJobName(model);
            JobKey jobkey = new JobKey(JobName, JOBGROUP);

            return IsJobExist(jobkey);
        }

        private static Boolean IsJobExist(JobKey jobkey)
        {
            Boolean result = false;
            try
            {
                if (_scheduler == null)
                {
                    InitSchedule();
                }

                IJobDetail tmpObject = _scheduler.GetJobDetail(jobkey);
                if (tmpObject != null)
                {
                    result = true;
                }
            }
            catch (SchedulerException e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 删除一个任务
        /// </summary>
        /// <param name="JobName"></param>
        /// <returns></returns>
        public static bool DeleteOneJob(QB_JOB_CONFIG_INFO model)
        {
            bool res = true;//成功
            try
            {
                string JobName = GetJobName(model);
                JobKey jobkey = new JobKey(JobName, JOBGROUP);
                if (IsJobExist(jobkey))
                {
                    if (_scheduler == null)
                    {
                        InitSchedule();
                    }

                    _scheduler.DeleteJob(jobkey);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                res = false;//失败
            }
            return res;
        }

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="JobName"></param>
        /// <returns></returns>
        public static bool AddOneJob(QB_JOB_CONFIG_INFO model)
        {
            bool res = true;//成功
            try
            {
                string JobName = GetJobName(model);
                JobKey jobkey = new JobKey(JobName, JOBGROUP);

                if (!IsJobExist(jobkey))
                {
                    QuartzAddJob(model, JobName);
                }
            }
            catch (Exception e)
            {
                //log.error(" 添加任务 执行周期为分 addOneJob 出错:", ex);
                Console.WriteLine(e.Message);
                res = false;//失败
            }
            return res;
        }

        /// <summary>
        /// 追加一个任务调度
        /// </summary>
        /// <param name="model"></param>
        /// <param name="JobName"></param>
        private static void QuartzAddJob(QB_JOB_CONFIG_INFO model, string JobName)
        {
            IJobDetail jobDetail = null;
            ITrigger trigger = null;

            // 创建job
            jobDetail = JobBuilder.Create<QK_AutoJob>()
                .WithIdentity(JobName, JOBGROUP)
                .Build();

            jobDetail.JobDataMap.Put("JobConfigModel", model);

            if (!string.IsNullOrEmpty(model.EXCUTE_MINUTE) && !string.IsNullOrEmpty(model.EXCUTE_HOUR))
            {
                string cronExpression = string.Format("0 {0} {1} * * ?", Convert.ToInt32(model.EXCUTE_MINUTE).ToString(), Convert.ToInt32(model.EXCUTE_HOUR).ToString());

                // 创建trigger
                trigger = TriggerBuilder.Create()
                    .WithIdentity(JobName + Trigger, JOBGROUP)
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
                   .WithIdentity(JobName + Trigger, JOBGROUP)
                   .ForJob(jobDetail.Key)
                   .WithDailyTimeIntervalSchedule(a => a.StartingDailyAt(stime).EndingDailyAt(etime).WithIntervalInMinutes(Interval))
                   .Build();
            }
            if (_scheduler == null)
            {
                InitSchedule();
            }

            _scheduler.ScheduleJob(jobDetail, trigger);
        }

        /// <summary>
        /// 取得任务名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string GetJobName(QB_JOB_CONFIG_INFO model)
        {
            string jobName = "";

            // 额度类型为空
            if (string.IsNullOrEmpty(model.AMT_TYPE))
            {
                jobName = model.JOB_TYPE;
            }
            else
            {
                jobName = model.JOB_TYPE + "_" + model.AMT_TYPE;
            }
            return jobName;
        }
    }
}
