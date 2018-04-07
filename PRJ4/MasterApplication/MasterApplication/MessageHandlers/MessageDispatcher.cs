using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterApplication.Models;
using MasterApplication.Threads;
using RabbitMQ.Client.Events;

namespace MasterApplication.MessageHandlers
{
    public class MessageDispatcher
    {
	    private IPublisher _publisher;
	    public MessageDispatcher(IPublisher publisher, IListener listener)
	    {
		    _publisher = publisher;

		    listener.OnMessageReceived += OnMessageReceived_DispatchMessage;
		}

	    private void OnMessageReceived_DispatchMessage(object sender, BasicDeliverEventArgs e)
	    {
		    var message = Encoding.UTF8.GetString(e.Body); //Message 
		    string senderModule = e.RoutingKey.Split('.').First(); //Extracts the module from which the message originated from
		    string topic = e.RoutingKey.Substring(e.RoutingKey.IndexOf('.') + 1); //Extract the topic without the sender module name

		    //Console.WriteLine("Dispatcher recived message from listener: \"{0}\" from module: {1}", message, senderModule);
		    //Console.WriteLine("Passing message to MessageHandler");

			try
		    {
			    IMessageHandler msgHandler = MessageHandlerFactory.GetMessageHandler(senderModule);
			    msgHandler.HandleMessage(message, _publisher, topic);
			}
		    catch (MessageHandlerCreationException ex)
		    {
			    Console.WriteLine(ex.Message);
		    }
		    
		    
	    }
    }
}
