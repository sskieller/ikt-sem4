using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime.CredentialManagement;
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
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("hosting.json", optional: true)
                .AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseUrls("https://fwps.azurewebsite.net/api/")
                .UseConfiguration(config)
                .Configure(app => { app.Run(context => context.Response.WriteAsync("Hello, World")); })
                .Build();

            //WebHost.CreateDefaultBuilder(args)
            //    .UseStartup<Startup>()
            //    .Build();
        }
            
    }
}
