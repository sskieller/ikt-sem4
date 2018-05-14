namespace SmartGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prosumers",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Produced = c.Single(nullable: false),
                        Consumed = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Prosumers");
        }
    }
}
