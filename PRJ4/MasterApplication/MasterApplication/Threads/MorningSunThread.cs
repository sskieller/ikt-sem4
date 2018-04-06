using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MasterApplication.Threads
{
    public class MorningSunThread : IDisposable
    {
        private readonly IModel _channel;
        private string _queueName;
        private EventingBasicConsumer _consumer;
        public MorningSunThread(IConnection connection)
        {
            _channel = connection.CreateModel();

            _queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(
                queue: _queueName,
                exchange: "amq.topic",
                routingKey: "MorningSun.#");

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += ReceivedMessageHandler;


        }
        public void Start()
        {
            _channel.BasicConsume(
                queue: _queueName,
                autoAck: true,
                consumer: _consumer);
        }


        public void Dispose()
        {
            _channel.Dispose();
        }

        private static void ReceivedMessageHandler(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Recived: {0}", message);
        }
    }
}
