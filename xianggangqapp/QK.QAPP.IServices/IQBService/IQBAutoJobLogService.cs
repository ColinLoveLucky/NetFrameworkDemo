using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;

namespace QK.QAPP.IServices
{
    public interface IQBAutoJobLogService
    {
        /// <summary>
        /// 获取自动任务日志列表
        /// </summary>
        /// <param name="jobloglistpara">查询参数对象</param>
        /// <returns></returns>
        PageData<QB_AUTO_JOB_LOG> GetAutoJobLogList(AutoJobLogListPara jobloglistpara);

    }
}
