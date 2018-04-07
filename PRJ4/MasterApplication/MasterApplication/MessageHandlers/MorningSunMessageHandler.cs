using System;
using System.Collections.Generic;
using System.Text;
using MasterApplication.Threads;

namespace MasterApplication.MessageHandlers
{
    public class MorningSunMessageHandler : IMessageHandler
    {
		//Create map of topics here
        public void HandleMessage(string message, IPublisher publisher, string topic = null)
        {
	        if (topic != null)
	        {
				if (topic == "tester")
					Console.WriteLine("Morning Sun: Tester message: {0}\n", message);
				else
				{
					Console.WriteLine("Morning Sun: Non-tester message: {0}\n", message);
				}
			}
            
        }
    }
}
