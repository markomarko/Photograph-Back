namespace Photograph.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Photos", "UserId");
            AddColumn("dbo.Photos", "UserId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Photos", "UserId", c => c.Int(nullable: false));
        }
    }
}
