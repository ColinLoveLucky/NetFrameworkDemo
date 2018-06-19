using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public enum ContractOPTYPE
    {
        /// <summary>
        /// 上传
        /// </summary>
        [Description("协议上传")]
        Contract_OP_INPUT = 1,
        /// <summary>
        /// 确认
        /// </summary>
        [Description("协议确认")]
        Contract_OP_CONFIRM = 2,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("协议上传")]
        Contract_OP_MODIFY = 3,
        /// <summary>
        /// 下载
        /// </summary>
        [Description("协议下载")]
        Contract_OP_DOWNLOAD = 4,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("协议删除")]
        Contract_OP_DELETE = 5,
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("协议驳回")]
        Contract_OP_REJECT = 6
    }
}
