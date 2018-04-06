using System;

namespace MasterApplication.MessageHandlers
{
    public class SnapBoxMessageHandler : IMessageHandler
    {
        public void HandleMessage(string message)
        {
            Console.WriteLine("Snap Box message: {0}", message);
        }
    }
}