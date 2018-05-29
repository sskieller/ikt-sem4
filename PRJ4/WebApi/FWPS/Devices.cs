using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FWPS
{
    /////////////////////////////////////////////////
    /// Static class for accessing SignalR devices hub
    /////////////////////////////////////////////////
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

    /////////////////////////////////////////////////
    /// Implementation of SignalR Hub class
    /////////////////////////////////////////////////
    public class DevicesHub : Hub
    {
        /////////////////////////////////////////////////
        /// Callback for new SignalR connection
        /////////////////////////////////////////////////
        public override Task OnConnectedAsync()
        {
            Devices.ConnectedDevices.Add(Context.ConnectionId, "Device " + (Devices.ConnectedDevices.Count + 1));
            new DebugWriter().Write(Context.ConnectionId + " Connected  --  " + Devices.ConnectedDevices.Count + " devices connected");
            return base.OnConnectedAsync();
        }

        /////////////////////////////////////////////////
        /// Callback for SignalR disconnection 
        /////////////////////////////////////////////////
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Devices.ConnectedDevices.Remove(Context.ConnectionId);
            new DebugWriter().Write(Context.ConnectionId + " Disconnected  --  " + Devices.ConnectedDevices.Count + " devices connected");
            return base.OnDisconnectedAsync(exception);
        }

        /////////////////////////////////////////////////
        /// Sends message to topic 'Send'
        /////////////////////////////////////////////////
        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }

        /////////////////////////////////////////////////
        /// Updates SignalR connection name
        /////////////////////////////////////////////////
        public Task UpdateName(string name)
        {
            Devices.ConnectedDevices[Context.ConnectionId] = name;
            return Task.CompletedTask;
        }

        /////////////////////////////////////////////////
        /// Not used, Updates item condition
        /////////////////////////////////////////////////
        public Task UpdateEntityCondition(string entity, string value)
        {
            new DebugWriter().Write("Updated entity cond:    " + entity + " --- " + value);
            return Clients.All.InvokeAsync("UpdateEntityCondition", entity, value);
        }

        /////////////////////////////////////////////////
        /// Sends SignalR message to connected devices with passed parameters
        /////////////////////////////////////////////////
        public Task UpdateSpecific(string type, string target, object obj)
        {
            return Clients.All.InvokeAsync("UpdateSpecific", type, target, obj);
        }
    }
}
