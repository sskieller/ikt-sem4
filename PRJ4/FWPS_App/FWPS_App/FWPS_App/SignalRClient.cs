using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;

namespace FWPS_App
{
    public class SignalRClient
    {
        // Singleton

        private static SignalRClient _instance;

        public static SignalRClient Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SignalRClient();
                return _instance;
            }
        }

        public event EventHandler<LightEventArgs> OnLightChanged;

        private HubConnection _connection;

        private SignalRClient()
        {
            _connection = new HubConnectionBuilder().WithUrl("http://fwps.azurewebsites.net/devices").Build();
            _connection.StartAsync().Wait();

            _connection.On<();
        }
    }

    public class LightEventArgs : EventArgs
    {
        public bool State { get; set; }
        public LightEventArgs(bool state)
        {
            State = state;
        }
    }
}
