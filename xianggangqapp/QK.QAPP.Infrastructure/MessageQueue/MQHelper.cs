using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.MessageQueue
{
    public static class MQHelper
    {
        /// <summary>
        /// 发送MQ，会释放连接
        /// </summary>
        /// <param name="server">MQ服务器</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="queueName">队列名</param>
        /// <param name="content">消息</param>
        /// <returns></returns>
        public static bool Publish(string server, string user, string pwd, string queueName, string content)
        {
            bool ret = false;
            using (MQProducer producer = new MQProducer(server,
                            user,
                            pwd,
                            queueName))
            {
                ret = producer.Publish(content);
            }
            return ret;
        }

        public static bool Publish(string server, string user, string pwd, string queueName, string content,out string error)
        {
            bool ret = false;
            using (MQProducer producer = new MQProducer(server,
                            user,
                            pwd,
                            queueName))
            {
                ret = producer.Publish(content,out error);
            }
            return ret;
        }
    }
}
