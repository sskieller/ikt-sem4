using System;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}
