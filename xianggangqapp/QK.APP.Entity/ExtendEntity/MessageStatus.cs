using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 通知消息状态
    /// </summary>
    public enum MessageStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        UnProcess,
        /// <summary>
        /// 已处理
        /// </summary>
        Processed,
    }

    /// <summary>
    /// 消息级别
    /// </summary>
    public enum MessagePRIORTYLEVEL
    {
        /// <summary>
        /// 低
        /// </summary>
        LOW,
        /// <summary>
        /// 中
        /// </summary>
        MIDDLE,
        /// <summary>
        /// 高
        /// </summary>
        HIGHT
    }
}
