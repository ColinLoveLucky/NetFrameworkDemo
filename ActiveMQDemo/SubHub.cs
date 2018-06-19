using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveMQDemo
{
    public class SubHub
    {
        private IConnectionFactory _factory;

        public SubHub()
        {
            _factory = new ConnectionFactory("tcp://localhost:61616/");
        }

        public void Send()
        {
            using (IConnection connection = _factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IMessageProducer prod = session.CreateProducer(
                        new ActiveMQTopic("Hello"));
					ITextMessage message = prod.CreateTextMessage();
					message.Text = "Hello World";
					prod.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
				}
			}
        }

        public void Receive()
        {
			using (IConnection connection = _factory.CreateConnection())
			{
				connection.ClientId = "Hi";
				connection.Start();
				ISession session = connection.CreateSession();
				IMessageConsumer consumer = session.CreateConsumer(new ActiveMQTopic("Hello"));
				//consumer.Listener += Consumer_Listener;
				var messsage=consumer.Receive<string>();
			}
		}
        private void Consumer_Listener(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            Console.WriteLine("Receive:" + msg.Text);
        }
    }
}


