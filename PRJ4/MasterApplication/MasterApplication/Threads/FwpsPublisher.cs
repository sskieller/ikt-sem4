using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using RabbitMQ.Client;

namespace MasterApplication.Threads
{
    /////////////////////////////////////////////////
    /// Static class to handle sending messages to
    /// the connected modules. Uses RabbitMQ
    /////////////////////////////////////////////////
    public static class FwpsPublisher
	{
		private static IModel _channel;
	    private static bool _initialized;
	    private static readonly object _lock = new object();


	    /////////////////////////////////////////////////
	    /// Sets up the Publisher
	    /////////////////////////////////////////////////
        public static void Initialize(IConnection connection)
	    {
            lock(_lock)
	        if (!_initialized)
	        {
	            _channel = connection.CreateModel();
	            _initialized = true;
	        }
            else
                throw new PublisherInitializedException("Already initialized");
        }

	    /////////////////////////////////////////////////
	    /// Publishes a message. Takes a topic & a
	    /// message as inputs. Topic tells it where to
	    /// route it. Message is a message.
	    /////////////////////////////////////////////////
        public static void PublishMessage(string topic, string message, string exchange = "amq.topic")
		{
            lock(_lock)
		    if (!_initialized)
		        throw new PublisherInitializedException("Not initialized");

			_channel.BasicPublish(
				exchange: exchange,
				routingKey: topic,
				basicProperties: null,
				body: Encoding.UTF8.GetBytes(message));
		}
	}

    public class PublisherInitializedException : Exception
    {
        public PublisherInitializedException(string message)
            : base(message)
        {

        }
    }
}
