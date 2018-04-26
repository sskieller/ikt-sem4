using System;
using System.Collections.Generic;
using System.Text;
using MasterApplication.Threads;
using RabbitMQ.Client;

namespace MasterApplication.Models
{
    public class FwpsMessageEventArgs : EventArgs
    {
	    public FwpsMessageEventArgs(string senderModule, string topic, string messsage)
	    {
		    SenderModule = senderModule;
		    Topic = topic;
		    Message = Message;
	    }


	    public string SenderModule { get; set; }
		public string Topic { get; set; }
		public string Message { get; set; }

    }
}
