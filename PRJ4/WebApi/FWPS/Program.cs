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
            //DebugWriter.Clear();
            DebugWriter debugWriter = new DebugWriter();

            debugWriter.Write("Starting...");
            debugWriter.Write("Running Server...");

            //Task t = Server.SetupServer();

            debugWriter.Write("Running App...");

            BuildWebHost(args).Run();

            //t.Wait();

            //Task.Run(() => { Server.SetupServer(); });
        }
        /////////////////////////////////////////////////
        /// This method builds webhost
        /////////////////////////////////////////////////
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }

    }
    
}
