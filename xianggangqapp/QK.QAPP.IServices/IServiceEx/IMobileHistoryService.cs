using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IMobileHistoryService
    {
        /// <summary>
        /// 从接口获取通话详单状态
        /// </summary>
        /// <param name="preAppCode">预申请编号</param>
        /// <returns></returns>
        string GetStatusFormApi(string preAppCode);


        /// <summary>
        /// 获取APP_MAIN中通话详单状态
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        string GetStatus(long appId);
    }
}
