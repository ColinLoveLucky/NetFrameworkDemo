using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;
using Microsoft.Practices.Unity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.Infrastructure.MessageQueue;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Web;
using QK.JobExcute;

namespace QK.QAPP.Services
{

    public class QBQuartzService : IQBQuartzService
    {
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="JobName"></param>
        /// <returns></returns>
        public bool QuartzAddOneJob(QB_JOB_CONFIG_INFO model)
        {
            return QuartzClinet.AddOneJob(model);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="JobName"></param>
        /// <returns></returns>
        public bool QuartzDeleteOneJob(QB_JOB_CONFIG_INFO model)
        {
            return QuartzClinet.DeleteOneJob(model);
        }

        public bool QuartzCheckJobsExist(QB_JOB_CONFIG_INFO model)
        {
            bool result = false;
            try
            {
                result = QuartzClinet.CheckJobExist(model);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

    }
}
