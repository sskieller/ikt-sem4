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
	    public MessageDispatcher(IListener listener)
	    {

		    listener.OnMessageReceived += OnMessageReceived_DispatchMessage;

            SignalRClient.Instance.OnCommandReceived += SignalROnCommandReceived;
		}

        private void SignalROnCommandReceived(object sender, SignalREventArgs signalREventArgs)
        {
            try
            {
                IMessageHandler msgHandler = MessageHandlerFactory.GetMessageHandler(signalREventArgs.Module);
                msgHandler.HandleMessage(signalREventArgs.Obj.ToString(), signalREventArgs.Topic);
            }
            catch (MessageHandlerCreationException ex)
            {
                Console.WriteLine(ex.Message);
            }
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
	            msgHandler.HandleMessage(message, topic);
	        }
	        catch (Exception ex)
	        {
	            Console.WriteLine(ex.Message);
	        }
		    
		    
	    }
    }
}
