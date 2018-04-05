using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FWPS
{
    public class Clients
    {
        // Singleton
        private static Clients _instance;

        public static Clients Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Clients();
                return _instance;
            }
        }
        private Clients() { }

        // Implementation

        //public static List<WebSocket> ConnectedSockets = new List<WebSocket>();
        private List<WebSocket> _connectedSockets = new List<WebSocket>();

        //private WebSocket _connectedSockets;
        //private Thread _thread;

        public async void AddClient(HttpContext webSocket)
        {
            var socket = await webSocket.WebSockets.AcceptWebSocketAsync();
            _connectedSockets.Add(socket);

            DebugWriter.Write("Added socket");
   
            SendToClient(socket, "Hello websocket client");

            // Needed to not terminate the connection. Its ok since it ran as async
            // and therefor is running on the threadpool
            while (socket.State == WebSocketState.Open || socket.State == WebSocketState.Connecting) { } 
        }

        public void SendToClient(WebSocket client, string message)
        {
            Task.Factory.StartNew(() => PushToClient(client, message));
        }

        public void SendToClients(string message)
        {  
            DebugWriter.Write(_connectedSockets.Count.ToString());
            foreach (var client in _connectedSockets)
            {
                if (client.State == WebSocketState.Open)
                {
                    Task.Factory.StartNew(() => PushToClient(client, message));
                }
                else
                {
                    DebugWriter.Write("Removed a socket");
                    _connectedSockets.Remove(client);
                }
            }
        }

        private async Task PushToClient(WebSocket webSocket, string message)
        {
            byte[] response = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(response, WebSocketMessageType.Text, true, CancellationToken.None);
        }     
    }
}