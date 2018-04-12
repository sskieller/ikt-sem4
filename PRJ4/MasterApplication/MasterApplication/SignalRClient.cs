using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace MasterApplication
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



        // Implementation

        public event EventHandler<SignalREventArgs> OnCommandReceived;

        private readonly HubConnection _connection;

        private SignalRClient()
        {
            _connection = new HubConnectionBuilder().WithUrl("http://fwps.azurewebsites.net/devices").Build();
            _connection.StartAsync().Wait();

            _connection.On<string, string, object>("UpdateSpecific", ((type, target, obj) =>
            {
                OnCommandReceived?.Invoke(this, new SignalREventArgs(obj, target, type));
            }));
        }


        /// <summary>
        /// Setups and configures the signalR connection to the Azure server
        /// </summary>
        /// <param name="deviceName">Name Displayed on the Azure server</param>
        /// <returns>void</returns>
        public void UpdateName(string deviceName)
        {
            _connection.SendAsync("UpdateName", deviceName).Wait();
        }
    }

    public class SignalREventArgs : EventArgs
    {
        public object Obj { get; set; }
        public string Topic { get; set; }
        public string Module { get; set; }
        public SignalREventArgs(object obj, string topic, string module)
        {
            Obj = obj;
            Topic = topic;
            Module = module;
        }
    }
}
