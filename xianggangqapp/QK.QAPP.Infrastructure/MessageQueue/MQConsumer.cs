using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using QK.QAPP.Infrastructure.Log4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.MessageQueue
{
    /// <summary>
    /// ApacheMQ订阅
    /// </summary>
    public class MQConsumer:IDisposable
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
        protected AutoResetEvent Semaphore = new AutoResetEvent(false);
        protected TimeSpan ReceiveTimeout;
        public delegate void MessageListener(MessageBody message);
        public event MessageListener Listener;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverHost">服务器</param>
        /// <param name="port">端口</param>
        /// <param name="queueName">Queue名称</param>
        public MQConsumer(string serverHost, string port, string queueName)
        {
            this.ServerHost = serverHost;
            this.Port = port;
            this.QueueName = string.Format(@"queue://"+queueName);
            connecturi = new Uri(string.Format(@"activemq:tcp://{0}:{1}", 
                ServerHost, Port));
            this.ReceiveTimeout = TimeSpan.FromSeconds(10);
            this.Init();
        }

        public MQConsumer(string serverHost,string UserName,string Password, string port, string queueName)
        {
            this.ServerHost = serverHost;
            this.UserName = UserName;
            this.Password = Password;
            this.Port = port;
            this.QueueName = string.Format(@"queue://" + queueName);
            connecturi = new Uri(string.Format(@"activemq:tcp://{0}:{1}",
                ServerHost, Port));
            this.ReceiveTimeout = TimeSpan.FromSeconds(10);
            this.Init();
        }

        public MQConsumer(string strConnection, string UserName, string Password, string queueName)
        {
            this.connecturi = new Uri(strConnection);
            this.UserName = UserName;
            this.Password = Password;
            this.QueueName = string.Format(@"queue://" + queueName);
            this.ReceiveTimeout = TimeSpan.FromSeconds(10);

            //新开一个线程，用于监听MQ的初始化(防止阻碍申请系统的使用) update by zhanghao on 2016-05-13
            Thread thread = new Thread(new ThreadStart(Init));
            thread.Start();
            //this.Init();
        }
        
        public MQConsumer(string strConnection,string queueName)
        {
            this.connecturi = new Uri(strConnection);
            this.QueueName = string.Format(@"queue://" + queueName);
            this.ReceiveTimeout = TimeSpan.FromSeconds(10);
            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {         
            try
            {
                //this.Factory = new NMSConnectionFactory(connecturi);
                this.Factory = new ConnectionFactory(connecturi);
                if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                {
                    this.Connection = Factory.CreateConnection(UserName,Password);
                }
                else
                {
                    this.Connection = Factory.CreateConnection();
                }            
                this.Connection.Start();            
                this.Session = Connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                this.Destination = SessionUtil.GetDestination(this.Session, this.QueueName);            
                IMessageConsumer consumer = Session.CreateConsumer(Destination);
                consumer.Listener += consumer_Listener;
            }
            catch(Exception ex)
            {
                LogWriter.Error("MQ启动错误", ex);
            }
            //consumer.Receive();
        }

        public void consumer_Listener(IMessage message)
        {
            MessageBody msg = new MessageBody();
            msg.Text = (message as ITextMessage).Text;
            Listener(msg);            
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
