using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 量化派信息处理状态
    /// </summary>
    public enum QuantStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        UnProcessed = 1,
        /// <summary>
        /// 处理成功
        /// </summary>
        Success,
        /// <summary>
        /// 处理失败
        /// </summary>
        Fail,
        /// <summary>
        /// 已下载
        /// </summary>
        Downloaded
    }
}
