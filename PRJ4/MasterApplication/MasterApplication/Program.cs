using System;
using System.Runtime.InteropServices;
using System.Text;
using MasterApplication.MessageHandlers;
using MasterApplication.Threads;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace MasterApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server application:\n");

            var connFactory = new ConnectionFactory() { HostName = "localhost" };
            connFactory.AutomaticRecoveryEnabled = true;

	        {
				MorningSunMessageHandler msgHandler = new MorningSunMessageHandler();
	        }



			using (var connection = connFactory.CreateConnection())
			{
				IListener listener = new FwpsListener(connection);
                FwpsPublisher.Initialize(connection);
				

				MessageDispatcher dispatcher = new MessageDispatcher(listener);

                //Add topics to subscribe to
				listener.Add("MorningSun.#");
				listener.Add("SnapBox.#");
				listener.Start();

				Console.WriteLine("Enter a char, 1 for on, 2 for off, q for quit");
				char key = Console.ReadKey().KeyChar;
				while (key != 'q')
				{
					if (key == '1')
					{
						FwpsPublisher.PublishMessage("SnapBox.CmdOn", "Hello");
						Console.WriteLine("Sending on");
					}
					else if (key == '2')
					{
						FwpsPublisher.PublishMessage("SnapBox.CmdOff", "Hello");
						Console.WriteLine("Sending off");
					}
					key = Console.ReadKey().KeyChar;
				}


				Console.WriteLine("Press Enter to Exit");
				Console.ReadLine();

				listener.Dispose();
			}


        }

    }

}
