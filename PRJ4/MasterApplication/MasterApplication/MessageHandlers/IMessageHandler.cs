using System;
using System.Collections.Generic;
using System.Text;

namespace MasterApplication.MessageHandlers
{
    public interface IMessageHandler
    {
        void HandleMessage(string message);
    }
}
