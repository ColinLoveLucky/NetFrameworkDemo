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
    public interface IJobBase
    {
        void CreateJobs();
    }
}
