using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace MasterApplication.Threads
{
    public interface IPublisher : IDisposable
    {
	    event EventHandler OnMessagePublished;
	    void PublishMessage(string topic, string message, string exchange = "amq.topic");
    }

	public class FwpsPublisher : IPublisher
	{
		private readonly IModel _channel;

		public FwpsPublisher(IConnection connection)
		{
			_channel = connection.CreateModel();
		}
		public void Dispose()
		{
			_channel.Dispose();
		}

		public event EventHandler OnMessagePublished;
		public void PublishMessage(string topic, string message, string exchange = "amq.topic")
		{
			_channel.BasicPublish(
				exchange: exchange,
				routingKey: topic,
				basicProperties: null,
				body: Encoding.UTF8.GetBytes(message));
		}
	}
}
