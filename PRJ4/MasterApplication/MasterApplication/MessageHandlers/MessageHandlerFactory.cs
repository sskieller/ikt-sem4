using System;
using System.Collections.Generic;
using System.Text;

namespace MasterApplication.MessageHandlers
{
    /////////////////////////////////////////////////
    /// Factory that dispends MessageHandlers.
    /////////////////////////////////////////////////
    public static class MessageHandlerFactory
    {
        /////////////////////////////////////////////////
        /// Spawn a MessageHandler according to requested
        /// type.
        /////////////////////////////////////////////////
        public static IMessageHandler GetMessageHandler(string messageHandlerType)
        {
            switch (messageHandlerType)
            {
                case "MorningSun":
                    return new MorningSunMessageHandler();
                case "SnapBox":
                    return new SnapBoxMessageHandler();
                case "Hodoor":
                    return new HodoorMessageHandler();
                case "Poomba":
                    return new PoombaMessageHandler();
                default:
                    throw new MessageHandlerCreationException("Type not recognized by MessageHandlerFactory");
            }
        }
    }
    public class MessageHandlerCreationException : Exception
    {
        public MessageHandlerCreationException(string message)
        :base(message)
        { 
        }
    }
}
