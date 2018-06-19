using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 通话详单获取状态
    /// </summary>
    public enum MobileHistoryStatus
    {
        /// <summary>
        /// 无数据需要执行
        /// </summary>
        Start = 1,
        /// <summary>
        /// 完成
        /// </summary>
        Finish,
        /// <summary>
        /// 获取失败
        /// </summary>
        Fail,
        /// <summary>
        /// 已获取推送数据
        /// </summary>
        Push
    }
}
