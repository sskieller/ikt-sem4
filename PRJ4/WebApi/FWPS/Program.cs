using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FWPS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DebugWriter.Clear();

            DebugWriter.Write("Starting...");
            DebugWriter.Write("Running Server...");

            Task t = Server.SetupServer();

            DebugWriter.Write("Running App...");

            BuildWebHost(args).Run();

            t.Wait();

            //Task.Run(() => { Server.SetupServer(); });
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            /*
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("hosting.json", optional: true)
                .AddCommandLine(args)
                .Build();

            

            return WebHost.CreateDefaultBuilder(args)
                .UseUrls("https://fwps.azurewebsites.net/api/")
                .UseConfiguration(config)
                .Configure(app => { app.Run(context => context.Response.WriteAsync("Hello, World")); })
                .Build();
            */
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }

    }

    public class Server
    {
        public static async Task SetupServer()
        {
            DebugWriter.Write("Intro to SetupServer");
            DebugWriter.Write("Making Server...");
            TcpListener server = new TcpListener(IPAddress.Parse("52.138.196.70"), 443);
            DebugWriter.Write("Starting Server...");
            server.Start();
            DebugWriter.Write("Server has started on 52.138.196.70:443. -- Waiting for a connection...");

            TcpClient client = server.AcceptTcpClient();

            DebugWriter.Write("A client connected.");

            NetworkStream stream = client.GetStream();



            //enter to an infinite cycle to be able to handle every change in stream
            while (true)
            {
                while (!stream.DataAvailable);

                Byte[] bytes = new Byte[client.Available];

                

                await stream.ReadAsync(bytes, 0, bytes.Length);

                //translate bytes of request to string
                String data = Encoding.UTF8.GetString(bytes);

                if (new Regex("^GET").IsMatch(data))
                {
                    DebugWriter.Write(data);

                    const string eol = "\r\n"; // HTTP/1.1 defines the sequence CR LF as the end-of-line marker

                    Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
                                                                                                + "Connection: Upgrade" +
                                                                                                eol
                                                                                                + "Upgrade: websocket" +
                                                                                                eol
                                                                                                + "Sec-WebSocket-Accept: " +
                                                                                                Convert.ToBase64String(
                                                                                                    System.Security
                                                                                                        .Cryptography
                                                                                                        .SHA1.Create()
                                                                                                        .ComputeHash(
                                                                                                            Encoding
                                                                                                                .UTF8
                                                                                                                .GetBytes(
                                                                                                                    new
                                                                                                                            System
                                                                                                                            .Text
                                                                                                                            .RegularExpressions
                                                                                                                            .Regex(
                                                                                                                                "Sec-WebSocket-Key: (.*)")
                                                                                                                        .Match(
                                                                                                                            data)
                                                                                                                        .Groups
                                                                                                                            [1]
                                                                                                                        .Value
                                                                                                                        .Trim() +
                                                                                                                    "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                                                                                                )
                                                                                                        )
                                                                                                ) + eol
                                                                                                + eol);

                    await stream.WriteAsync(response, 0, response.Length);

                }
                else if (bytes[0] != 138)
                {
                    DebugWriter.Write(GetMessage(bytes));
                    string responseString = GetMessage(bytes);

                    byte[] response = DataFrame(WebSocketDatatype.Text, Encoding.UTF8.GetBytes(responseString));

                    //DebugWriter.Write(bytes.Length);

                    await stream.WriteAsync(response, 0, response.Length);
                }
            }
        }

        private static string GetMessage(Byte[] bytes)
        {
            string DETEXT = "";
            if (bytes[0] == 129)
            {
                int position = 0;
                int Type = 0;
                ulong length = 0;
                if (bytes[1] - 128 >= 0 && bytes[1] - 128 <= 125)
                {
                    length = (ulong) bytes[1] - 128;
                    position = 2;
                }
                else if (bytes[1] - 128 == 126)
                {
                    Type = 1;
                    length = (ulong) 256 * bytes[2] + bytes[3];
                    position = 4;
                }
                else if (bytes[1] - 128 == 127)
                {
                    Type = 2;
                    for (int i = 0; i < 8; i++)
                    {
                        ulong pow = Convert.ToUInt64(Math.Pow(256, (7 - i)));
                        length = length + bytes[2 + i] * pow;
                        position = 10;
                    }
                }
                else
                {
                    Type = 3;
                    DebugWriter.Write("error 1");
                }

                if (Type < 3)
                {
                    Byte[] key = new Byte[4]
                        {bytes[position], bytes[position + 1], bytes[position + 2], bytes[position + 3]};
                    Byte[] decoded = new Byte[bytes.Length - (4 + position)];
                    Byte[] encoded = new Byte[bytes.Length - (4 + position)];

                    for (long i = 0; i < bytes.Length - (4 + position); i++) encoded[i] = bytes[i + position + 4];

                    for (int i = 0; i < encoded.Length; i++) decoded[i] = (Byte) (encoded[i] ^ key[i % 4]);

                    DETEXT = Encoding.UTF8.GetString(decoded);
                }
            }
            else
            {
                DebugWriter.Write("error 2: " + bytes[0].ToString());
            }

            return DETEXT;
        }

        public enum WebSocketDatatype
        {
            Continuation = 0x00,
            Text = 0x01,
            Binary = 0x02,
            ConnectionClose = 0x08
        }

        public static byte[] DataFrame(WebSocketDatatype dataType, byte[] payload, bool isLastFrame = true)
        {
            MemoryStream memoryStream = new MemoryStream();


            // If its the last frame, set the FIN bit
            byte finAndOpcodeByte = isLastFrame ? (byte) 0x80 : (byte) 0x00;


            // Opcode aka. the Datatype is presented in here
            byte firstByte = (byte) (finAndOpcodeByte | (byte) dataType);


            // Writing to the mem buffer.
            memoryStream.WriteByte(firstByte);

            if (payload.Length < 126)
            {
                byte secondByte = (byte) payload.Length;
                memoryStream.WriteByte(secondByte);
            }
            else if (payload.Length <= ushort.MaxValue)
            {
                byte secondByte = (byte) 126;

                memoryStream.WriteByte(secondByte);

                ushort len = (ushort) payload.Length;

                byte upper = (byte) ((len >> 8) & 0xff);

                byte lower = (byte) (len & 0xff);

                memoryStream.WriteByte(upper);
                memoryStream.WriteByte(lower);
            }
            else
            {
                // Wat pls no
            }

            foreach (var bytes in payload)
                memoryStream.WriteByte(bytes);

            return memoryStream.ToArray();

        }
    }
}
