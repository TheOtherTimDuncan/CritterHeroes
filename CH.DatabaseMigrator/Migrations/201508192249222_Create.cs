namespace CH.DatabaseMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organization",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    FullName = c.String(nullable: false, maxLength: 100),
                    ShortName = c.String(maxLength: 50),
                    AzureName = c.String(nullable: false, maxLength: 25, unicode: false),
                    LogoFilename = c.String(maxLength: 255, unicode: false),
                    EmailAddress = c.String(nullable: false, maxLength: 255),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.AppRole",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.AppUserRole",
                c => new
                {
                    UserId = c.Int(nullable: false),
                    RoleId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new
                {
                    t.UserId,
                    t.RoleId
                })
                .ForeignKey("dbo.AppRole", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.AppUser",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(maxLength: 50),
                    LastName = c.String(maxLength: 50),
                    NewEmail = c.String(maxLength: 256),
                    UserName = c.String(nullable: false, maxLength: 256),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AppUserClaim",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AppUserLogin",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new
                {
                    t.LoginProvider,
                    t.ProviderKey,
                    t.UserId
                })
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AnimalStatus",
                c => new
                {
                    ID = c.Int(nullable: false),
                    Name = c.String(maxLength: 25),
                    Description = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);

            CreateTable(
                "dbo.Breed",
                c => new
                {
                    ID = c.Int(nullable: false),
                    SpeciesID = c.Int(nullable: false),
                    BreedName = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Species", t => t.SpeciesID, cascadeDelete: true)
                .Index(t => t.SpeciesID);

            CreateTable(
                "dbo.Species",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 50),
                    Singular = c.String(maxLength: 50),
                    Plural = c.String(maxLength: 50),
                    YoungSingular = c.String(maxLength: 50),
                    YoungPlural = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Breed", "SpeciesID", "dbo.Species");
            DropForeignKey("dbo.AppUserRole", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserLogin", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserClaim", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserRole", "RoleId", "dbo.AppRole");
            DropIndex("dbo.Species", new[] { "Name" });
            DropIndex("dbo.Breed", new[] { "SpeciesID" });
            DropIndex("dbo.AnimalStatus", new[] { "Name" });
            DropIndex("dbo.AppUserLogin", new[] { "UserId" });
            DropIndex("dbo.AppUserClaim", new[] { "UserId" });
            DropIndex("dbo.AppUser", "UserNameIndex");
            DropIndex("dbo.AppUserRole", new[] { "RoleId" });
            DropIndex("dbo.AppUserRole", new[] { "UserId" });
            DropIndex("dbo.AppRole", "RoleNameIndex");
            DropTable("dbo.Species");
            DropTable("dbo.Breed");
            DropTable("dbo.AnimalStatus");
            DropTable("dbo.AppUserLogin");
            DropTable("dbo.AppUserClaim");
            DropTable("dbo.AppUser");
            DropTable("dbo.AppUserRole");
            DropTable("dbo.AppRole");
            DropTable("dbo.Organization");
        }
    }
}
