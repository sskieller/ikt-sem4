using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace MasterApplication
{
    /////////////////////////////////////////////////
    /// SignalRClient is a communicationsHub which is
    /// responsible for communicating with the WebApi.
    /// Singleton Class so that it's avaliable everywhere.
    /// Observer pattern, since you can subscribe to
    /// different kind of functions/topics.
    /////////////////////////////////////////////////
    public class SignalRClient
    {
        // Singleton

        private static SignalRClient _instance; //!< Singleton instance of SignalRClient

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

        /////////////////////////////////////////////////
        /// The eventhandler which is called when a
        /// message is received.
        /////////////////////////////////////////////////
        public event EventHandler<SignalREventArgs> OnCommandReceived;

        /////////////////////////////////////////////////
        /// The Setup function. Needs to be called when
        /// the program starts. Sets up the internal
        /// SignalR class, subscribes to topics and
        /// updates the unit name on the Web Api.
        /////////////////////////////////////////////////
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

        private HubConnection _connection; //!< This is the connection variable. Stores an instance of the actual connection.

        private SignalRClient()
        { }


        /////////////////////////////////////////////////
        /// Sets up the connection to the azure hub.
        /// Will also subscripe to the UpdateSpecific
        /// topic. Starts the connection and adds an
        /// event that handles if the connection is lost.
        /////////////////////////////////////////////////
        private void SetUpInternal()
        {
            _connection = new HubConnectionBuilder().WithUrl("http://fwps.azurewebsites.net/devices").Build();

            _connection.On<string, string, object>("UpdateSpecific", ((type, target, obj) =>
            {
                OnCommandReceived?.Invoke(this, new SignalREventArgs(obj, target, type));
            }));

            _connection.StartAsync().Wait();

            _connection.Closed += ConnectionOnClosed;
        }

        /////////////////////////////////////////////////
        /// Handles if the connection closes. Tries to
        /// connect again after 15 seconds.
        /// Will try for 15 seconds, and retry if failed.
        /////////////////////////////////////////////////
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

    /////////////////////////////////////////////////
    /// EventArgs passed to the eventhandler function.
    /////////////////////////////////////////////////
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
