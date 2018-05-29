using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWPS.Models;
using Microsoft.EntityFrameworkCore;

namespace FWPS.Data
{
    /////////////////////////////////////////////////
    /// DbContext containing Mapping between Model classes and database
    /////////////////////////////////////////////////
    public class FwpsDbContext : DbContext
    {
        public DbSet<ClimateControlItem> ClimateControlItems { get; set; }
        public DbSet<CurtainItem> CurtainItems { get; set; }
        public DbSet<HodoorItem> HodoorItems { get; set; }
        public DbSet<IpItem> IpItems { get; set; }
        
        public DbSet<LightItem> LightItems { get; set; }
        public DbSet<LoginItem> LoginItems { get; set; }
        public DbSet<MailItem> MailItems { get; set; }
        public DbSet<PoombaItem> PoombaItems { get; set; }
        public DbSet<SnapBoxItem> SnapBoxItems { get; set; }

        public FwpsDbContext()
        {
            // Testing constructor
        }

        public FwpsDbContext(DbContextOptions<FwpsDbContext> options)
            : base(options)
        {

        }
    }
}
