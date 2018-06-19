/***********************
 * 作    者：刘云松
 * 创建时间：‎2014‎-10‎-‎29 ‏‎18:25:13
 * 作    用：提供补件记录相关的功能
*****************************/
using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IAPPQueueLogService
    {
        /// <summary>
        /// 添加AppQueueLog
        /// </summary>
        /// <param name="appQueueLog"></param>
        /// <returns></returns>
        void AddAppQueueLog(APPQueueLog appQueueLog);
    }
}
