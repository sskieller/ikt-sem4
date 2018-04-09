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
				IPublisher publisher = new FwpsPublisher(connection);

				MessageDispatcher dispatcher = new MessageDispatcher(publisher, listener);

				listener.Add("MorningSun.#");
				listener.Start();



				Console.WriteLine("Press Enter to remove morning sun and add snap box");
				Console.ReadLine();

				listener.Add("SnapBox.#");
				listener.Remove("MorningSun.#");


				Console.WriteLine("Press Enter to Add morning sun again");
				Console.ReadLine();

				listener.Add("MorningSun.#");


				Console.WriteLine("Press Enter to Exit");
				Console.ReadLine();

				listener.Dispose();
			}


        }

    }

}
