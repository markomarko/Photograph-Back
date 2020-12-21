namespace Photograph.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialBranch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        ClientId = c.String(),
                        ClientSecrets = c.String(),
                        Flow = c.Int(nullable: false),
                        AccessTokenLifetime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Scopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IncludeAllClaimsForUser = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlbumId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Context = c.String(),
                        Selected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.AlbumId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 255),
                        Password = c.String(),
                        Email = c.String(maxLength: 255),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DependsOnAdminId = c.Int(nullable: false),
                        ValidUntil = c.Double(nullable: false),
                        SubscriberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Subscribers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsSuspended = c.Boolean(nullable: false),
                        IsTrial = c.Boolean(nullable: false),
                        StripeId = c.String(),
                        SubscriptionPlan = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScopeClients",
                c => new
                    {
                        Scope_Id = c.Int(nullable: false),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scope_Id, t.Client_Id })
                .ForeignKey("dbo.Scopes", t => t.Scope_Id, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Scope_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Role_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Photos", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.ScopeClients", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ScopeClients", "Scope_Id", "dbo.Scopes");
            DropIndex("dbo.UserRoles", new[] { "Role_Id" });
            DropIndex("dbo.UserRoles", new[] { "User_Id" });
            DropIndex("dbo.ScopeClients", new[] { "Client_Id" });
            DropIndex("dbo.ScopeClients", new[] { "Scope_Id" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Photos", new[] { "AlbumId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.ScopeClients");
            DropTable("dbo.Subscribers");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Photos");
            DropTable("dbo.Scopes");
            DropTable("dbo.Clients");
            DropTable("dbo.Albums");
        }
    }
}
