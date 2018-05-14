using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartGrid
{
    public class EventNotifier
    {
        private static readonly object Locker = new object();
        // Singleton
        private static EventNotifier _instance;
        public static EventNotifier Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(Locker)
                        if(_instance == null)
                            _instance = new EventNotifier();
                }
                return _instance;
            }
        }

        // EventHandlers
        public EventHandler ProsumersUpdatedEventHandler { get; private set; }

        private EventNotifier()
        {
            ProsumersUpdatedEventHandler += ProsumersUpdatedHandler;
        }

        private void ProsumersUpdatedHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}