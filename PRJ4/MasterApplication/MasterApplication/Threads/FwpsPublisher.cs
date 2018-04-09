using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace MasterApplication.Threads
{

	public static class FwpsPublisher
	{
		private static IModel _channel;
	    private static bool _initialized;



	    public static void Initialize(IConnection connection)
	    {
            if (!_initialized)
	            _channel = connection.CreateModel();
        }


		public static void PublishMessage(string topic, string message, string exchange = "amq.topic")
		{
		    if (!_initialized)
		        return;

			_channel.BasicPublish(
				exchange: exchange,
				routingKey: topic,
				basicProperties: null,
				body: Encoding.UTF8.GetBytes(message));
		}
	}
}
