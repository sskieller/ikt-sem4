namespace SmartGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Spelling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prosumers", "Difference", c => c.Single(nullable: false));
            DropColumn("dbo.Prosumers", "Differece");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prosumers", "Differece", c => c.Single(nullable: false));
            DropColumn("dbo.Prosumers", "Difference");
        }
    }
}
