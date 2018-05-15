namespace SmartGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedtimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SmartGridModels", "TimeStamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SmartGridModels", "TimeStamp");
        }
    }
}
