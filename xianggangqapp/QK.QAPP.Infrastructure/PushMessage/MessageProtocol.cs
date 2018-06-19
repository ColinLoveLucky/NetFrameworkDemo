using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS.Util;

namespace QK.QAPP.Infrastructure.MessageQueue
{
    /// <summary>
    /// 数据交换协议
    /// </summary>
    [Serializable]
    public class MessageProtocol
    {
        /// <summary>
        /// 发布来源
        /// </summary>
        public string FromUser { get; set; }
        /// <summary>
        /// 接收者
        /// </summary>
        public string ToUser { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息实体
        /// </summary>
        public string Body { get; set; }



    }
}
