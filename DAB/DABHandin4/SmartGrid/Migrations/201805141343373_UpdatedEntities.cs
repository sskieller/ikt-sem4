namespace SmartGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedEntities : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Prosumers");
            AddColumn("dbo.Prosumers", "Differece", c => c.Single(nullable: false));
            AlterColumn("dbo.Prosumers", "Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Prosumers", "Name");
            DropColumn("dbo.Prosumers", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prosumers", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Prosumers");
            AlterColumn("dbo.Prosumers", "Name", c => c.String());
            DropColumn("dbo.Prosumers", "Differece");
            AddPrimaryKey("dbo.Prosumers", "Id");
        }
    }
}
