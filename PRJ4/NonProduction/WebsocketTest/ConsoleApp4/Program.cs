using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class program
    {
        private static bool Run = true;

        public static void Main(string[] args)
        {
            //Console.WriteLine(ushort.MaxValue);
            Task t = Echo();

            t.Wait();
            /*
            while (true)
            {
                if (Console.Read() == 'q')
                {
                    Run = false;
                    System.Environment.Exit(0);
                }
            }
            */
        }

        private static async Task Echo()
        {
            using (ClientWebSocket ws = new ClientWebSocket())
            {
                Uri serverUri = new Uri("ws://fwps.azurewebsites.net:80/ws");
                await ws.ConnectAsync(serverUri, CancellationToken.None);
                while (ws.State == WebSocketState.Open)
                {
                    /*
                    Console.Write("Input message ('exit' to exit): ");
                    string msg = Console.ReadLine();
                    if (msg == "exit")
                    {
                        break;
                    }
                    ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
                    await ws.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
                    */
                    ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);
                    WebSocketReceiveResult result = await ws.ReceiveAsync(bytesReceived, CancellationToken.None);
                    
                    
                    Console.WriteLine(Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count));
                }
            }
        }
    }
}