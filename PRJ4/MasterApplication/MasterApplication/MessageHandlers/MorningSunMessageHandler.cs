using System;
using System.Collections.Generic;
using System.Text;

namespace MasterApplication.MessageHandlers
{
    public class MorningSunMessageHandler : IMessageHandler
    {
        public void HandleMessage(string message)
        {
            Console.WriteLine("Morning Sun message: {0}", message);
        }
    }
}
