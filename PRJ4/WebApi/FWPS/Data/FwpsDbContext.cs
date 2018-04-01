using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWPS.Models;
using Microsoft.EntityFrameworkCore;

namespace FWPS.Data
{
    public class FwpsDbContext : DbContext
    {
        public DbSet<LightItem> LightItems { get; set; }
        public DbSet<LoginItem> LoginItems { get; set; }
        public DbSet<IpItem> IpItems { get; set; }
        public DbSet<SnapBoxItem> SnapBoxItems { get; set; }
        public DbSet<MailItem> MailItems { get; set; }

        public FwpsDbContext(DbContextOptions<FwpsDbContext> options)
            : base(options)
        {

        }
    }
}
