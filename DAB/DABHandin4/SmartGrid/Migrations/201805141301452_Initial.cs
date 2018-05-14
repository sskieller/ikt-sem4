namespace SmartGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prosumers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Produced = c.Single(nullable: false),
                        Consumed = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SmartGridModels",
                c => new
                    {
                        SmartGridId = c.Int(nullable: false, identity: true),
                        TotalForbrug = c.Double(nullable: false),
                        TotalGeneration = c.Double(nullable: false),
                        Brutto = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SmartGridId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SmartGridModels");
            DropTable("dbo.Prosumers");
        }
    }
}
