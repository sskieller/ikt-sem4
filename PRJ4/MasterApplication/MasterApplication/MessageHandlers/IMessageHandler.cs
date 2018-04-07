using System;
using System.Collections.Generic;
using System.Text;
using MasterApplication.Threads;

namespace MasterApplication.MessageHandlers
{
    public interface IMessageHandler
    {
        void HandleMessage(string message, IPublisher publisher, string topic = null);
    }
}
