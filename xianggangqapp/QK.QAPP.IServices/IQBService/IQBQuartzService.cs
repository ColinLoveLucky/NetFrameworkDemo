using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;

namespace QK.QAPP.IServices
{
    public interface IQBQuartzService
    {
        bool QuartzAddOneJob(QB_JOB_CONFIG_INFO model);

        bool QuartzDeleteOneJob(QB_JOB_CONFIG_INFO model);

        bool QuartzCheckJobsExist(QB_JOB_CONFIG_INFO model);
    }
}
