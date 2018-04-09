using System;
using MasterApplication.Threads;

namespace MasterApplication.MessageHandlers
{
    public class SnapBoxMessageHandler : IMessageHandler
    {
        public void HandleMessage(string message, string topic = null)
        {
            Console.WriteLine("Snap Box message: {0}", message);
        }
    }
}