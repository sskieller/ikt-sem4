using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWPS.Models;
using Microsoft.EntityFrameworkCore;

namespace FWPS.Data
{
    public class FWPS_DB_Context : DbContext
    {
        public DbSet<LightItem> LightItems { get; set; }
        public DbSet<LoginItem> LoginItems { get; set; }
        public DbSet<IpItem> IpItems { get; set; }

        public FWPS_DB_Context(DbContextOptions<FWPS_DB_Context> options)
            : base(options)
        {

        }
    }
}
