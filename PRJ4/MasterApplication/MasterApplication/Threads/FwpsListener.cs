using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterApplication.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MasterApplication.Threads
{
    /////////////////////////////////////////////////
    /// Class that listens for messages using RabbitMQ
    /// Will throw a new event if it receives a message.
    /////////////////////////////////////////////////
    public class FwpsListener : IListener
    {
	    public event EventHandler<BasicDeliverEventArgs> OnMessageReceived;
		private readonly IModel _channel;
        private string _queueName;
	    private List<Tuple<string, string>> _routingKeys; // <RoutingKey> <Exchange>
        private EventingBasicConsumer _consumer;

	    public bool IsActive { get; private set; }

	    public FwpsListener(IConnection connection)
        {
            _channel = connection.CreateModel();
            _queueName = _channel.QueueDeclare().QueueName;
			_routingKeys = new List<Tuple<string, string>>();

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += ReceivedMessageHandler;


        }

	    /// <summary>
	    /// Starts the Listener, allowing for messages to be received
	    /// </summary>
	    /// <returns>void</returns>
		public void Start()
        {

		    _channel.BasicConsume(
			    queue: _queueName,
			    autoAck: true,
			    consumer: _consumer);
	        IsActive = true;
        }

	    public void Stop()
	    {
		    _channel.BasicCancel(_queueName);
		    IsActive = false;
	    }


	    /// <summary>
	    /// Adds routing key to listener, allowing Receiver to listen to keys denoted by 'routingKey'. Please use '*' to denote one word wildcards and '#' for zero or multiple word wildcards
	    /// </summary>
	    /// <returns>void</returns>
		public void Add(string routingKey, string exchange = "amq.topic")
	    {
		    if (_routingKeys.Any(t => t.Item1 == routingKey && t.Item2 == exchange))
			    return; //Key exists already
			
			_routingKeys.Add(Tuple.Create(routingKey, exchange));

			_channel.QueueBind(
				queue: _queueName,
				exchange: exchange,
				routingKey: routingKey);
	    }
	    /// <summary>
	    /// Removed routing key from listener, allowing Receiver to listen to keys denoted by 'routingKey'. Please use '*' to denote one word wildcards and '#' for zero or multiple word wildcards
	    /// </summary>
	    /// <returns>void</returns>
		public void Remove(string routingKey, string exchange = "amq.topic")
	    {
		    _routingKeys.Remove(Tuple.Create(routingKey, exchange));

			_channel.QueueUnbind(
				queue: _queueName,
				exchange: exchange,
				routingKey: routingKey);

	    }


	    /// <summary>
	    /// Dispose object, closing channel
	    /// </summary>
	    /// <returns>void</returns>
		public void Dispose()
        {
            _channel.Dispose();
        }

	    /// <summary>
	    /// Raises 'OnMessageReceived' event, passing event information using FwpsMessageEventArgs
	    /// </summary>
	    /// <returns>void</returns>
		private void ReceivedMessageHandler(object sender, BasicDeliverEventArgs e)
	    {
			OnMessageReceived?.Invoke(this, e);
		}
    }

}
