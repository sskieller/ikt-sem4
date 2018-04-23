namespace Handin32.Data
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class AddressModel : DbContext
	{
		public AddressModel()
			: base("name=AddressModel")
		{
		}

		public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
		public virtual DbSet<Emails> Emails { get; set; }
		public virtual DbSet<People> People { get; set; }
		public virtual DbSet<PhoneNumbers> PhoneNumbers { get; set; }
		public virtual DbSet<PublicAddresses> PublicAddresses { get; set; }
		public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<People>()
				.HasMany(e => e.Emails)
				.WithOptional(e => e.People)
				.HasForeignKey(e => e.Person_Id);

			modelBuilder.Entity<People>()
				.HasMany(e => e.PhoneNumbers)
				.WithOptional(e => e.People)
				.HasForeignKey(e => e.Person_Id);

			modelBuilder.Entity<People>()
				.HasMany(e => e.PublicAddresses)
				.WithMany(e => e.People)
				.Map(m => m.ToTable("PublicAddressPersons").MapLeftKey("Person_Id").MapRightKey("PublicAddress_Id"));
		}
	}
}
