namespace Photograph.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        OwnerId = c.Guid(nullable: false),
                        Owner_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subscribers", t => t.Owner_UserId)
                .Index(t => t.Owner_UserId);
            
            CreateTable(
                "dbo.Subscribers",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        IsSuspended = c.Boolean(nullable: false),
                        IsTrial = c.Boolean(nullable: false),
                        StripeId = c.String(),
                        SubscriptionPlan = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(maxLength: 255),
                        Password = c.String(),
                        Email = c.String(maxLength: 255),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ValidUntil = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AlbumId = c.Guid(nullable: false),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Context = c.String(),
                        Selected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.AlbumId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Guid(nullable: false),
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
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        IncludeAllClaimsForUser = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.PhotoUsers",
                c => new
                    {
                        Photo_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Photo_Id, t.User_Id })
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Photo_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.SubscriberUsers",
                c => new
                    {
                        Subscriber_UserId = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subscriber_UserId, t.User_Id })
                .ForeignKey("dbo.Subscribers", t => t.Subscriber_UserId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Subscriber_UserId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AlbumUsers",
                c => new
                    {
                        Album_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Album_Id, t.User_Id })
                .ForeignKey("dbo.Albums", t => t.Album_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Album_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ScopeClients",
                c => new
                    {
                        Scope_Id = c.Guid(nullable: false),
                        Client_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scope_Id, t.Client_Id })
                .ForeignKey("dbo.Scopes", t => t.Scope_Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Scope_Id)
                .Index(t => t.Client_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScopeClients", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ScopeClients", "Scope_Id", "dbo.Scopes");
            DropForeignKey("dbo.AlbumUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AlbumUsers", "Album_Id", "dbo.Albums");
            DropForeignKey("dbo.Subscribers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Albums", "Owner_UserId", "dbo.Subscribers");
            DropForeignKey("dbo.SubscriberUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.SubscriberUsers", "Subscriber_UserId", "dbo.Subscribers");
            DropForeignKey("dbo.PhotoUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PhotoUsers", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.Photos", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropIndex("dbo.ScopeClients", new[] { "Client_Id" });
            DropIndex("dbo.ScopeClients", new[] { "Scope_Id" });
            DropIndex("dbo.AlbumUsers", new[] { "User_Id" });
            DropIndex("dbo.AlbumUsers", new[] { "Album_Id" });
            DropIndex("dbo.SubscriberUsers", new[] { "User_Id" });
            DropIndex("dbo.SubscriberUsers", new[] { "Subscriber_UserId" });
            DropIndex("dbo.PhotoUsers", new[] { "User_Id" });
            DropIndex("dbo.PhotoUsers", new[] { "Photo_Id" });
            DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.Photos", new[] { "AlbumId" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Subscribers", new[] { "UserId" });
            DropIndex("dbo.Albums", new[] { "Owner_UserId" });
            DropTable("dbo.ScopeClients");
            DropTable("dbo.AlbumUsers");
            DropTable("dbo.SubscriberUsers");
            DropTable("dbo.PhotoUsers");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.Scopes");
            DropTable("dbo.Clients");
            DropTable("dbo.Photos");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Subscribers");
            DropTable("dbo.Albums");
        }
    }
}
