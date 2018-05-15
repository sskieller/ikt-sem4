namespace SmartGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class simon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prosumers", "Remainder", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prosumers", "Remainder");
        }
    }
}
