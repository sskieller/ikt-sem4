namespace SmartGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prosumers", "PreferedBuyerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prosumers", "PreferedBuyerName");
        }
    }
}
