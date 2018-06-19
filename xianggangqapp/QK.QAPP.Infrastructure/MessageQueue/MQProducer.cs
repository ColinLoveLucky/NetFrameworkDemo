using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.MessageQueue
{
    /// <summary>
    /// ApacheMQ发布
    /// </summary>
    public class MQProducer:IDisposable
    {
        private string ServerHost;
        private string Port;
        private string QueueName;
        private string UserName;
        private string Password;

        private Uri connecturi;
        private IConnectionFactory Factory;
        private IConnection Connection;
        private ISession Session;
        private IDestination Destination;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverHost"></param>
        /// <param name="port"></param>
        public MQProducer(string serverHost, string port, string queueName)
        {
            this.ServerHost = serverHost;
            this.Port = port;
            this.QueueName = string.Format(@"queue://"+queueName);
            connecturi = new Uri(string.Format(@"activemq:tcp://{0}:{1}", 
                ServerHost, Port));
            this.Init();
        }

        public MQProducer(string serverHost, string UserName, string Password, string port, string queueName)
        {
            this.ServerHost = serverHost;
            this.UserName = UserName;
            this.Password = Password;
            this.Port = port;
            this.QueueName = string.Format(@"queue://" + queueName);
            connecturi = new Uri(string.Format(@"activemq:tcp://{0}:{1}",
                ServerHost, Port));
            this.Init();
        }

        public MQProducer(string strConnection, string UserName, string Password, string queueName)
        {
            this.connecturi = new Uri(strConnection);
            this.UserName = UserName;
            this.Password = Password;
            this.QueueName = string.Format(@"queue://" + queueName);
            this.Init();
        }

        public MQProducer(string strConnection, string queueName)
        {
            this.connecturi = new Uri(strConnection);
            this.QueueName = string.Format(@"queue://" + queueName);
            this.Init();
        }

        private void Init()
        {
            this.Factory = new ConnectionFactory(connecturi);
            this.Connection = Factory.CreateConnection();
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
            {
                this.Connection = Factory.CreateConnection(UserName,Password);
            }
            else
            {
                this.Connection = Factory.CreateConnection();
            }   
            this.Session = Connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            this.Destination = SessionUtil.GetDestination(this.Session, this.QueueName);
            this.Connection.Start();   
        }

        public bool Publish(string content)
        {
            bool ret = false;
            try
            {
                ITextMessage msg = this.Session.CreateTextMessage();
                msg.Text = content;
                IMessageProducer producer = Session.CreateProducer(Destination);                             
                producer.Send(msg, MsgDeliveryMode.Persistent, MsgPriority.High, TimeSpan.MinValue);
                ret = true;
            }
            catch (Exception)
            {
                return ret;
            }
            return ret;            
        }

        public bool Publish(string content, out string errorMsg)
        {
            errorMsg = "";
            bool ret = false;
            try
            {
                ITextMessage msg = this.Session.CreateTextMessage();
                msg.Text = content;
                IMessageProducer producer = Session.CreateProducer(Destination);
                producer.Send(msg, MsgDeliveryMode.Persistent, MsgPriority.High, TimeSpan.MinValue);
                ret = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return ret;
            }
            return ret;
        }  

        /// <summary>
        /// 注销连接
        /// </summary>
        public void Dispose()
        {
            if (this.Session!=null)
            {
                this.Session.Close();
                this.Session.Dispose();
            }
            if (this.Connection!=null)
            {
                this.Connection.Close();
                this.Connection.Dispose();
            }
        }
    }
}
