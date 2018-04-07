using System;
using System.Collections.Generic;
using System.Text;
using MasterApplication.Models;
using RabbitMQ.Client.Events;

namespace MasterApplication.Threads
{
    public interface IListener : IDisposable
    {
	    event EventHandler<BasicDeliverEventArgs> OnMessageReceived;
	    void Start();
	    void Stop();
	    void Add(string routingKey, string exchange = "amq.topic");
	    void Remove(string routingKey, string exchange = "amq.topic");
    }
}
