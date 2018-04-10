using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FWPS.Models;
using Microsoft.EntityFrameworkCore;
using FWPS.Data;
using Microsoft.AspNetCore.Http;

namespace FWPS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<LightContext>(opt => opt.UseInMemoryDatabase("LightItem"));
            var connectionString =
                @"Server=fwps.database.windows.net;Database=FWPS_DB;User Id=dbadmin;Password=Navyseal1";
            services.AddDbContext<FwpsDbContext>(opt => opt.UseSqlServer(connectionString));
            //services.AddDbContext<LightContext>(opt => opt.UseSqlServer(connectionString));
	        //services.AddDbContext<IpContext>(opt => opt.UseInMemoryDatabase("IpItem"));
            //services.AddDbContext<LoginContext>(opt => opt.UseInMemoryDatabase("LoginItem"));
			services.AddMvc();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseMvc();

            // SignalR Routing
            app.UseSignalR(route => route.MapHub<DevicesHub>("devices"));

            // Commented out for now, implementing SignalR instead. It's way more reliable on Azure servers
            /*
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                { 
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        Clients.Instance.AddClient(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });
            */
        }
    }
}
