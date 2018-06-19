using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public partial interface IQuotaUsageService
    {
        /// <summary>
        /// 获取额度使用情况
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        string GetAmtUseSummary(AmtAssignListPara para);
    }
}
