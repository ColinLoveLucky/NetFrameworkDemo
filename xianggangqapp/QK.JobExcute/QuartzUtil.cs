//using QK.JobCommon;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
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
    public class QuartzUtil
    {
        private static IScheduler _scheduler;
        private static String JOBGROUP = "QKJobGroup";// 日程组名称；
        private static String Trigger = "Trigger";// 日程组名称；

        public const string QAPP = "qapp";
        public const string QKEY = "qk2016";

        public static IScheduler Scheduler
        {
            get
            {
                return _scheduler;
            }
        }

        static QuartzUtil()
        {
            try
            {
                var properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "RemoteServerScheduler";

                // 设置线程池
                properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
                properties["quartz.threadPool.threadCount"] = "10";
                properties["quartz.threadPool.threadPriority"] = "Normal";

                // 远程输出配置
                properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
                properties["quartz.scheduler.exporter.port"] = "600";
                properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
                properties["quartz.scheduler.exporter.channelType"] = "tcp";

                ISchedulerFactory schedulerFactory = new StdSchedulerFactory(properties);
                _scheduler = schedulerFactory.GetScheduler();// 生成日程对象；
            }
            catch (SchedulerException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 初始化所有任务
        /// </summary>
        public static void SchedulerAllJob()
        {
            try
            {
                var strQBAPI = ConfigurationSettings.AppSettings["QBAPIADDRESS"].ToString();

                var rest = new RestApiHelper(strQBAPI + GlobalApi.QKAutoJobConfig);
                var paras = SecuritySignHelper.GetSecurityCollectionWithSign(null, QAPP, QKEY);

                List<QB_JOB_CONFIG_INFO> listBidJobConfig = rest.Get<List<QB_JOB_CONFIG_INFO>>("GetJobConfigInfoList", paras);
                if (listBidJobConfig != null && listBidJobConfig.Count > 0)
                {
                    IJobBase jobCreate = new JobAllCreate(Scheduler, listBidJobConfig);
                    // 创建任务
                    jobCreate.CreateJobs();
                    //LogHelper.WriteLog("读取任务配置，自动任务开始执行");
                }
                else
                {
                    // LogHelper.WriteLog("读取配置信息失败，请配置任务信息或检查网络接口");
                }
            }
            catch (Exception ex)
            {
                // LogHelper.WriteLog("读取任务信息异常", ex);
            }
        }

        /// <summary>
        /// 任务开始
        /// </summary>
        public static void StartScheduler()
        {
            try
            {
                _scheduler.Start();
            }
            catch (Exception ex)
            {
                // LogHelper.WriteLog("任务异常开始", ex);
            }
        }

        /// <summary>
        /// 任务结束
        /// </summary>
        public static void StopScheduler()
        {
            try
            {
                _scheduler.Shutdown();
            }
            catch (Exception ex)
            {
                // LogHelper.WriteLog("任务异常结束", ex);
            }
        }
    }
}
