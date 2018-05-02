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

        /// <summary>
        /// Setups and configures the signalR connection to the Azure server
        /// </summary>
        /// <param name="deviceName">Name Displayed on the Azure server</param>
        /// <returns>void</returns>
        public void Setup(string deviceName)
        {
            SetUpInternal();
            _connection.SendAsync("UpdateName", deviceName).Wait();
        }

        public void UpdateEntityCondition(string entity, string value)
        {
            _connection.SendAsync("UpdateEntityCondition", entity, value).Wait();
        }


        // Private implementation

        private HubConnection _connection;

        private SignalRClient()
        { }

        private void SetUpInternal()
        {
            _connection = new HubConnectionBuilder().WithUrl("http://fwps.azurewebsites.net/devices").Build();

            _connection.On<string, string, object>("UpdateSpecific", ((type, target, obj) =>
            {
                OnCommandReceived?.Invoke(this, new SignalREventArgs(obj, target, type));
            }));

            _connection.On<string, string>("UpdateEntityCondition", (entity, value) =>
            {
                Console.WriteLine(entity + " --- " + value);
            });

            _connection.StartAsync().Wait();

            _connection.Closed += ConnectionOnClosed;
        }

        private Task ConnectionOnClosed(Exception exception)
        {
            Console.WriteLine(exception.Message);

            Console.WriteLine("Connection closed -- Trying to reconnect");

            Task.Delay(TimeSpan.FromSeconds(15));

            //_connection = new HubConnectionBuilder().WithUrl("http://fwps.azurewebsites.net/devices").Build();
            /*
            try
            {
                _connection.StartAsync().Wait(TimeSpan.FromSeconds(15));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            */

            _connection.StartAsync().Wait(TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        private void TryToReconnect(int times)
        {
            if (times > 5) return;
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
