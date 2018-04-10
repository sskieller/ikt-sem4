using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FWPS
{
    public static class Devices
    {
        public static IHubContext<DevicesHub> Hub
        {
            get => Hub;
            set
            {
                if (Hub == null) Hub = value;
            }
        }

        public static Dictionary<string, string> ConnectedDevices = new Dictionary<string, string>();
    }

    public class DevicesHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Devices.ConnectedDevices.Add(Context.ConnectionId, "Device " + (Devices.ConnectedDevices.Count+1));
            DebugWriter.Write(Context.ConnectionId + " Connected  --  " + Devices.ConnectedDevices.Count + " devices connected");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Devices.ConnectedDevices.Remove(Context.ConnectionId);
            DebugWriter.Write(Context.ConnectionId + " Disconnected  --  " + Devices.ConnectedDevices.Count + " devices connected");
            return base.OnDisconnectedAsync(exception);
        }

        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }

        public Task Update(string type, object obj)
        {
            return Clients.All.InvokeAsync("Update", type, obj);
        }

        public Task UpdateSpecific(string type, string target, object obj)
        {
            return Clients.All.InvokeAsync("Update", type, target, obj);
        }
    }
}
