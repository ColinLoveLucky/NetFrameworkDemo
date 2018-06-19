using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.Log4Net
{
    /// <summary>
    /// 业务日志
    /// </summary>
    public class BizLog
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 函数名
        /// </summary>
        public string Function { get; set; }
        /// <summary>
        /// 机器名
        /// </summary>
        public string Machine { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 实体
        /// </summary>
        public string Entity { get; set; }
    }

    /// <summary>
    /// 系统日志
    /// </summary>
    public class SysLog
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 主机名
        /// </summary>
        public string Host { get; set; }

    }
}
