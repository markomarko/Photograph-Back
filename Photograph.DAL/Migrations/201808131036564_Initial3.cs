namespace Photograph.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsSuspended", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "Suspended");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Suspended", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "IsSuspended");
        }
    }
}
