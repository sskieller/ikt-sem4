using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FWPS.Models
{
    public partial class FWPS_DBContext : DbContext
    {
        public virtual DbSet<LightObject> LightObject { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LightObject>(entity =>
            {
                entity.HasKey(e => e.LightId);

                entity.Property(e => e.Command)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });
        }

        public FWPS_DBContext(DbContextOptions<LightContext> options)
            : base(options)
        { }
    }
}
