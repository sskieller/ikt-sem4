using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
// using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            _connection = new HubConnection("http://fwps.azurewebsites.net/"); //HubConnectionBuilder().WithUrl("http://fwps.azurewebsites.net/devices").Build();

            IHubProxy devices = _connection.CreateHubProxy("devices");

            devices.On<string, string>("UpdateEntityCondition", (entity, value) =>
            {
                
            });
            /*
            _connection.On<string, object>("Update", (s, o) =>
            {
                if (s == "Light")
                {
                    var item = JsonConvert.DeserializeObject<LightPage.LightObject>(o.ToString());

                    OnLightChanged?.Invoke(this, new LightEventArgs(item.Command == "on"));
                }
            });
            */
        }
        /*
        public async Task UpdateName()
        {
            await _connection.SendAsync("Send", "Hello from mobile");
        }
        */
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
