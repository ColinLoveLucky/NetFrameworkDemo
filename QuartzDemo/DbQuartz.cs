using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace QuartzDemo
{
    //public class DbQuartz : ServiceControl, ServiceSuspend
    //{
    //    public DbQuartz()
    //    {
    //        //1.首先创建一个作业调度池
    //        var properties = new NameValueCollection();
    //        //存储类型
    //        properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
    //        //表明前缀
    //        properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
    //        //驱动类型
    //        properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";                //数据源名称
    //        properties["quartz.jobStore.dataSource"] = "myDs";
    //        //连接字符串

    //        properties["quartz.dataSource.myDS.connectionString"] = ConfigurationManager.AppSettings["DbDemo"];
    //        //sqlserver版本
    //        properties["quartz.dataSource.myDS.provider"] = "ystem.Data.SqlClient";
    //        //最大链接数
    //        //properties["quartz.dataSource.myDS.maxConnections"] = "5";
    //        // First we must get a reference to a scheduler
    //        ISchedulerFactory sf = new StdSchedulerFactory(properties);
    //        IScheduler sched = sf.GetScheduler();
    //    }
    //    public bool Continue(HostControl hostControl)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Pause(HostControl hostControl)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Start(HostControl hostControl)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Stop(HostControl hostControl)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class DbQuartz
    {
        public void Test()
        {
            NameValueCollection properties = new NameValueCollection()
            {
                {"quartz.scheduler.instanceName", "RemoteServerSchedulerClient"},
                {"quartz.threadPool.type","Quartz.Simpl.SimpleThreadPool, Quartz" },
                { "quartz.threadPool.threadCount","5"},
                {"quartz.scheduler.exporter.type" ,"Quartz.Simpl.RemotingSchedulerExporter, Quartz"},
                { "quartz.scheduler.exporter.port","556"},
                {"quartz.scheduler.exporter.bindName","QuartzScheduler" },
                {"quartz.scheduler.exporter.channelType","tcp"}
            };
            // 驱动类型，这里用的mysql，目前支持如下驱动：
            //Quartz.Impl.AdoJobStore.FirebirdDelegate
            //Quartz.Impl.AdoJobStore.MySQLDelegate
            //Quartz.Impl.AdoJobStore.OracleDelegate
            //Quartz.Impl.AdoJobStore.SQLiteDelegate
            //Quartz.Impl.AdoJobStore.SqlServerDelegate
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
            // 数据源名称
            properties["quartz.jobStore.dataSource"] = "myDS";
            // 数据库版本
            /* 数据库版本    MySql.Data.dll版本,二者必须保持一致
             * MySql-10    1.0.10.1
             * MySql-109   1.0.9.0
             * MySql-50    5.0.9.0
             * MySql-51    5.1.6.0
             * MySql-65    6.5.4.0
             * MySql-695   6.9.5.0
             *             System.Data
             * SqlServer-20         2.0.0.0
             * SqlServerCe-351      3.5.1.0
             * SqlServerCe-352      3.5.1.50
             * SqlServerCe-400      4.0.0.0
             * 其他还有OracleODP，Npgsql，SQLite，Firebird，OleDb
            */
            properties["quartz.dataSource.myDS.provider"] = "SqlServer-20";
            // 连接字符串
            properties["quartz.dataSource.myDS.connectionString"] = "Data Source=QF-XGZHANG-01\\SQLEXPRESS;Initial Catalog=DbQuartz;User ID=sa;Password=Password@1";
            // 事物类型JobStoreTX自动管理 JobStoreCMT应用程序管理
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            // 表明前缀
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";

            // Quartz Scheduler唯一实例ID，auto：自动生成
            properties["quartz.scheduler.instanceId"] = "AUTO";

            // 集群
            properties["quartz.jobStore.clustered"] = "true";

            ISchedulerFactory schedfDataBase = new StdSchedulerFactory(properties);
            IScheduler sched = schedfDataBase.GetScheduler();

            // 添加任务和触发器
            IJobDetail jobDetail = JobBuilder.Create<HelloJob>().WithIdentity("jobtest4", "group4").Build();
            IJobDetail jobDetail2 = JobBuilder.Create<HelloWorldJob>().WithIdentity("jobtest5", "group5").Build();

            ITrigger simpleTrigger = (ISimpleTrigger)TriggerBuilder.Create().WithIdentity("simpleTrigger4", "group4").WithSimpleSchedule(x => x.WithIntervalInSeconds(2).WithRepeatCount(5)).Build();

            ITrigger crontrigger = (ICronTrigger)TriggerBuilder.Create().WithIdentity("cronTrigger5", "group5").WithCronSchedule("0/5 * * * * ? ").Build();

            sched.ScheduleJob(jobDetail, crontrigger);

            sched.ScheduleJob(jobDetail2, simpleTrigger);

            // 开始调度
            sched.Start();
        }

   

       
    }
}
