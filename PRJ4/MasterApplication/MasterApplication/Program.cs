using System;
using System.Runtime.InteropServices;
using System.Text;
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



            using (var connection = connFactory.CreateConnection())
            {
                var thread = new MorningSunThread(connection);

                thread.Start();


                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();

                thread.Dispose();
            }


        }

    }

}
