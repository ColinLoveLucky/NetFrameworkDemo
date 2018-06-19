using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQDemo
{
	//ActiveMq 补发机制 ClientAcknowledge 模式 ，异步的方式异常了，会产生补发
	//PrefetchPolicy 设置没有处理完成，宕机会补发所有
	//Session 是事务类型 消费宕机会补发
	//IRedeliveryPolicy 补发的时间，重发的次数都是可以手动设置的
	public class ActiveMQConnect
	{
		private IConnectionFactory _factory;
		public ActiveMQConnect()
		{
			_factory = new ConnectionFactory("tcp://localhost:61616");
		}
		public void Send()
		{
			using (IConnection connection = _factory.CreateConnection())
			{
				using (ISession session = connection.CreateSession(AcknowledgementMode.Transactional))
				{
					try
					{
						IMessageProducer prod = session.CreateProducer(new ActiveMQQueue("firstQueue"));
						ITextMessage message = prod.CreateTextMessage();
						message.Text = "Hello World";
						message.Properties.SetString("filter", "demo");
						prod.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
						session.Commit();
					}
					catch(Exception ex)
					{
						Console.WriteLine("I'm RollBack");
						session.Rollback();
					}
				}
			}
		}
		public void Receive()
		{
			using (IConnection connection = _factory.CreateConnection())
			{
				connection.ClientId = "firstQueueListener";
				connection.Start();
				ISession session = connection.CreateSession();
				IMessageConsumer consumer = session.CreateConsumer(new ActiveMQQueue("firstQueue"), "filter='demo'");
				consumer.Listener += Consumer_Listener;
			}
		}
		private void Consumer_Listener(IMessage message)
		{
			ITextMessage msg = (ITextMessage)message;
			msg.Acknowledge();
			Console.WriteLine(msg.Text);
			//throw new Exception();
		}
	}
}
