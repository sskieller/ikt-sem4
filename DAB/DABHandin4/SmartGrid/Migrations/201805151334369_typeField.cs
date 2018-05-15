namespace SmartGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class typeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prosumers", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prosumers", "Type");
        }
    }
}
