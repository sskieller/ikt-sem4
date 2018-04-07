using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttTestClient
{
    class Program
    {
        private static MqttClient client;
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please enter the two arguments: <topic> <message>");
                return;
            }

            client = new MqttClient("localhost");

            string clientId = "Mqtt_tester";
            client.Connect(clientId, "simon", "simon");

            client.MqttMsgPublished += HandleMessagePublished;

            if (client.IsConnected)
            {
                Console.WriteLine("Connected, now sending message");
                client.Publish(args[0], Encoding.UTF8.GetBytes(args[1]), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                Console.WriteLine("Sent message: \"{0}\" to topic: \"{1}\"", args[1], args[0]);
                Thread.Sleep(1000);
                //client.Disconnect();
                
            }

        }

        private static void HandleMessagePublished(object sender, MqttMsgPublishedEventArgs mqttMsgPublishedEventArgs)
        {
            client.Disconnect();
        }
    }
}
