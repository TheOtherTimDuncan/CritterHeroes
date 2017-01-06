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
                    RescueGroupsID = c.Int(),
                    AzureName = c.String(nullable: false, maxLength: 25, unicode: false),
                    LogoFilename = c.String(maxLength: 255, unicode: false),
                    EmailAddress = c.String(nullable: false, maxLength: 255),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.OrganizationSupportedCritter",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    OrganizationID = c.Guid(nullable: false),
                    SpeciesID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Species", t => t.SpeciesID)
                .ForeignKey("dbo.Organization", t => t.OrganizationID, cascadeDelete: true)
                .Index(t => new
                {
                    t.OrganizationID,
                    t.SpeciesID
                }, unique: true, name: "IX_OrganizationSpecies");

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

            CreateTable(
                "dbo.Breed",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    SpeciesID = c.Int(nullable: false),
                    BreedName = c.String(nullable: false, maxLength: 100),
                    RescueGroupsID = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Species", t => t.SpeciesID, cascadeDelete: true)
                .Index(t => new
                {
                    t.SpeciesID,
                    t.BreedName
                }, name: "SpeciesBreed")
                .Index(t => t.RescueGroupsID);

            CreateTable(
                "dbo.Critter",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    RescueGroupsID = c.Int(),
                    OrganizationID = c.Guid(nullable: false),
                    StatusID = c.Int(nullable: false),
                    LocationID = c.Int(),
                    RescueGroupsLastUpdated = c.DateTimeOffset(precision: 7),
                    RescueGroupsCreated = c.DateTimeOffset(precision: 7),
                    WhenCreated = c.DateTimeOffset(nullable: false, precision: 7),
                    WhenUpdated = c.DateTimeOffset(nullable: false, precision: 7),
                    Name = c.String(nullable: false, maxLength: 50),
                    BreedID = c.Int(nullable: false),
                    Sex = c.String(nullable: false, maxLength: 10),
                    RescueID = c.String(maxLength: 100, unicode: false),
                    ReceivedDate = c.DateTime(storeType: "date"),
                    IsCourtesy = c.Boolean(nullable: false),
                    Description = c.String(),
                    GeneralAge = c.String(maxLength: 10, unicode: false),
                    HasSpecialNeeds = c.Boolean(nullable: false),
                    SpecialNeedsDescription = c.String(),
                    HasSpecialDiet = c.Boolean(nullable: false),
                    FosterID = c.Int(),
                    BirthDate = c.DateTime(storeType: "date"),
                    IsBirthDateExact = c.Boolean(),
                    EuthanasiaDate = c.DateTime(storeType: "date"),
                    EuthanasiaReason = c.String(maxLength: 15),
                    ColorID = c.Int(),
                    IsMicrochipped = c.Boolean(),
                    IsOkWithDogs = c.Boolean(),
                    IsOkWithKids = c.Boolean(),
                    IsOkWithCats = c.Boolean(),
                    OlderKidsOnly = c.Boolean(),
                    Notes = c.String(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Breed", t => t.BreedID)
                .ForeignKey("dbo.CritterColor", t => t.ColorID)
                .ForeignKey("dbo.Person", t => t.FosterID)
                .ForeignKey("dbo.Location", t => t.LocationID)
                .ForeignKey("dbo.Organization", t => t.OrganizationID)
                .ForeignKey("dbo.CritterStatus", t => t.StatusID)
                .Index(t => t.OrganizationID)
                .Index(t => t.StatusID)
                .Index(t => t.LocationID)
                .Index(t => t.Name)
                .Index(t => t.BreedID)
                .Index(t => t.FosterID)
                .Index(t => t.ColorID);

            CreateTable(
                "dbo.CritterColor",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    RescueGroupsID = c.Int(),
                    Description = c.String(nullable: false, maxLength: 100),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.RescueGroupsID);

            CreateTable(
                "dbo.Person",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    FirstName = c.String(maxLength: 100),
                    LastName = c.String(maxLength: 100),
                    Email = c.String(maxLength: 256),
                    NewEmail = c.String(maxLength: 256),
                    IsEmailConfirmed = c.Boolean(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                    Address = c.String(maxLength: 100),
                    City = c.String(maxLength: 100),
                    State = c.String(maxLength: 2, unicode: false),
                    Zip = c.String(maxLength: 10, unicode: false),
                    RescueGroupsID = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.RescueGroupsID);

            CreateTable(
                "dbo.PersonGroup",
                c => new
                {
                    PersonID = c.Int(nullable: false),
                    GroupID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new
                {
                    t.PersonID,
                    t.GroupID
                })
                .ForeignKey("dbo.Group", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID)
                .Index(t => t.GroupID);

            CreateTable(
                "dbo.Group",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    IsPerson = c.Boolean(nullable: false),
                    IsBusiness = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.PersonPhone",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    PersonID = c.Int(nullable: false),
                    PhoneNumber = c.String(nullable: false, maxLength: 10, unicode: false),
                    PhoneExtension = c.String(maxLength: 6, unicode: false),
                    PhoneTypeID = c.Int(),
                })
                .PrimaryKey(t => new
                {
                    t.ID,
                    t.PersonID
                })
                .ForeignKey("dbo.PhoneType", t => t.PhoneTypeID)
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID)
                .Index(t => t.PhoneTypeID);

            CreateTable(
                "dbo.PhoneType",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 10),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Location",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 50),
                    Address = c.String(maxLength: 50),
                    City = c.String(maxLength: 20),
                    State = c.String(maxLength: 2, unicode: false),
                    Zip = c.String(maxLength: 10, unicode: false),
                    Phone = c.String(maxLength: 14, unicode: false),
                    Website = c.String(maxLength: 200, unicode: false),
                    RescueGroupsID = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.RescueGroupsID);

            CreateTable(
                "dbo.CritterPicture",
                c => new
                {
                    CritterID = c.Int(nullable: false),
                    PictureID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new
                {
                    t.CritterID,
                    t.PictureID
                })
                .ForeignKey("dbo.Critter", t => t.CritterID, cascadeDelete: true)
                .ForeignKey("dbo.Picture", t => t.PictureID, cascadeDelete: true)
                .Index(t => t.CritterID)
                .Index(t => t.PictureID);

            CreateTable(
                "dbo.Picture",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Filename = c.String(nullable: false, maxLength: 256, unicode: false),
                    DisplayOrder = c.Int(),
                    Width = c.Int(nullable: false),
                    Height = c.Int(nullable: false),
                    FileSize = c.Long(nullable: false),
                    ContentType = c.String(nullable: false, maxLength: 256, unicode: false),
                    WhenCreated = c.DateTimeOffset(nullable: false, precision: 7),
                    RescueGroupsCreated = c.DateTimeOffset(precision: 7),
                    RescueGroupsID = c.Int(),
                    SourceUrl = c.String(maxLength: 1000, unicode: false),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.RescueGroupsID);

            CreateTable(
                "dbo.PictureChild",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ParentID = c.Int(nullable: false),
                    Filename = c.String(nullable: false, maxLength: 256, unicode: false),
                    Width = c.Int(nullable: false),
                    Height = c.Int(nullable: false),
                    FileSize = c.Long(nullable: false),
                    WhenCreated = c.DateTimeOffset(nullable: false, precision: 7),
                })
                .PrimaryKey(t => new
                {
                    t.ID,
                    t.ParentID
                })
                .ForeignKey("dbo.Picture", t => t.ParentID, cascadeDelete: true)
                .Index(t => t.ParentID);

            CreateTable(
                "dbo.CritterStatus",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 25),
                    Description = c.String(maxLength: 100),
                    RescueGroupsID = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true)
                .Index(t => t.RescueGroupsID);

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
                    PersonID = c.Int(nullable: false),
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
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID)
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
                "dbo.State",
                c => new
                {
                    Abbreviation = c.String(nullable: false, maxLength: 2, unicode: false),
                    Name = c.String(nullable: false, maxLength: 14, unicode: false),
                })
                .PrimaryKey(t => t.Abbreviation);

            CreateTable(
                "dbo.BusinessGroup",
                c => new
                {
                    BusinessID = c.Int(nullable: false),
                    GroupID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new
                {
                    t.BusinessID,
                    t.GroupID
                })
                .ForeignKey("dbo.Business", t => t.BusinessID, cascadeDelete: true)
                .ForeignKey("dbo.Group", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.BusinessID)
                .Index(t => t.GroupID);

            CreateTable(
                "dbo.Business",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 100),
                    Email = c.String(maxLength: 256),
                    Address = c.String(maxLength: 100),
                    City = c.String(maxLength: 100),
                    State = c.String(maxLength: 2, unicode: false),
                    Zip = c.String(maxLength: 10, unicode: false),
                    RescueGroupsID = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.RescueGroupsID);

            CreateTable(
                "dbo.BusinessPhone",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    BusinessID = c.Int(nullable: false),
                    PhoneNumber = c.String(nullable: false, maxLength: 10, unicode: false),
                    PhoneExtension = c.String(maxLength: 6, unicode: false),
                    PhoneTypeID = c.Int(),
                })
                .PrimaryKey(t => new
                {
                    t.ID,
                    t.BusinessID
                })
                .ForeignKey("dbo.PhoneType", t => t.PhoneTypeID)
                .ForeignKey("dbo.Business", t => t.BusinessID, cascadeDelete: true)
                .Index(t => t.BusinessID)
                .Index(t => t.PhoneTypeID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.BusinessGroup", "GroupID", "dbo.Group");
            DropForeignKey("dbo.BusinessPhone", "BusinessID", "dbo.Business");
            DropForeignKey("dbo.BusinessPhone", "PhoneTypeID", "dbo.PhoneType");
            DropForeignKey("dbo.BusinessGroup", "BusinessID", "dbo.Business");
            DropForeignKey("dbo.AppUserRole", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUser", "PersonID", "dbo.Person");
            DropForeignKey("dbo.AppUserLogin", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserClaim", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserRole", "RoleId", "dbo.AppRole");
            DropForeignKey("dbo.OrganizationSupportedCritter", "OrganizationID", "dbo.Organization");
            DropForeignKey("dbo.OrganizationSupportedCritter", "SpeciesID", "dbo.Species");
            DropForeignKey("dbo.Breed", "SpeciesID", "dbo.Species");
            DropForeignKey("dbo.Critter", "StatusID", "dbo.CritterStatus");
            DropForeignKey("dbo.CritterPicture", "PictureID", "dbo.Picture");
            DropForeignKey("dbo.PictureChild", "ParentID", "dbo.Picture");
            DropForeignKey("dbo.CritterPicture", "CritterID", "dbo.Critter");
            DropForeignKey("dbo.Critter", "OrganizationID", "dbo.Organization");
            DropForeignKey("dbo.Critter", "LocationID", "dbo.Location");
            DropForeignKey("dbo.PersonPhone", "PersonID", "dbo.Person");
            DropForeignKey("dbo.PersonPhone", "PhoneTypeID", "dbo.PhoneType");
            DropForeignKey("dbo.PersonGroup", "PersonID", "dbo.Person");
            DropForeignKey("dbo.PersonGroup", "GroupID", "dbo.Group");
            DropForeignKey("dbo.Critter", "FosterID", "dbo.Person");
            DropForeignKey("dbo.Critter", "ColorID", "dbo.CritterColor");
            DropForeignKey("dbo.Critter", "BreedID", "dbo.Breed");
            DropIndex("dbo.BusinessPhone", new[] { "PhoneTypeID" });
            DropIndex("dbo.BusinessPhone", new[] { "BusinessID" });
            DropIndex("dbo.Business", new[] { "RescueGroupsID" });
            DropIndex("dbo.BusinessGroup", new[] { "GroupID" });
            DropIndex("dbo.BusinessGroup", new[] { "BusinessID" });
            DropIndex("dbo.AppUserLogin", new[] { "UserId" });
            DropIndex("dbo.AppUserClaim", new[] { "UserId" });
            DropIndex("dbo.AppUser", "UserNameIndex");
            DropIndex("dbo.AppUser", new[] { "PersonID" });
            DropIndex("dbo.AppUserRole", new[] { "RoleId" });
            DropIndex("dbo.AppUserRole", new[] { "UserId" });
            DropIndex("dbo.AppRole", "RoleNameIndex");
            DropIndex("dbo.CritterStatus", new[] { "RescueGroupsID" });
            DropIndex("dbo.CritterStatus", new[] { "Name" });
            DropIndex("dbo.PictureChild", new[] { "ParentID" });
            DropIndex("dbo.Picture", new[] { "RescueGroupsID" });
            DropIndex("dbo.CritterPicture", new[] { "PictureID" });
            DropIndex("dbo.CritterPicture", new[] { "CritterID" });
            DropIndex("dbo.Location", new[] { "RescueGroupsID" });
            DropIndex("dbo.PersonPhone", new[] { "PhoneTypeID" });
            DropIndex("dbo.PersonPhone", new[] { "PersonID" });
            DropIndex("dbo.PersonGroup", new[] { "GroupID" });
            DropIndex("dbo.PersonGroup", new[] { "PersonID" });
            DropIndex("dbo.Person", new[] { "RescueGroupsID" });
            DropIndex("dbo.CritterColor", new[] { "RescueGroupsID" });
            DropIndex("dbo.Critter", new[] { "ColorID" });
            DropIndex("dbo.Critter", new[] { "FosterID" });
            DropIndex("dbo.Critter", new[] { "BreedID" });
            DropIndex("dbo.Critter", new[] { "Name" });
            DropIndex("dbo.Critter", new[] { "LocationID" });
            DropIndex("dbo.Critter", new[] { "StatusID" });
            DropIndex("dbo.Critter", new[] { "OrganizationID" });
            DropIndex("dbo.Breed", new[] { "RescueGroupsID" });
            DropIndex("dbo.Breed", "SpeciesBreed");
            DropIndex("dbo.Species", new[] { "Name" });
            DropIndex("dbo.OrganizationSupportedCritter", "IX_OrganizationSpecies");
            DropTable("dbo.BusinessPhone");
            DropTable("dbo.Business");
            DropTable("dbo.BusinessGroup");
            DropTable("dbo.State");
            DropTable("dbo.AppUserLogin");
            DropTable("dbo.AppUserClaim");
            DropTable("dbo.AppUser");
            DropTable("dbo.AppUserRole");
            DropTable("dbo.AppRole");
            DropTable("dbo.CritterStatus");
            DropTable("dbo.PictureChild");
            DropTable("dbo.Picture");
            DropTable("dbo.CritterPicture");
            DropTable("dbo.Location");
            DropTable("dbo.PhoneType");
            DropTable("dbo.PersonPhone");
            DropTable("dbo.Group");
            DropTable("dbo.PersonGroup");
            DropTable("dbo.Person");
            DropTable("dbo.CritterColor");
            DropTable("dbo.Critter");
            DropTable("dbo.Breed");
            DropTable("dbo.Species");
            DropTable("dbo.OrganizationSupportedCritter");
            DropTable("dbo.Organization");
        }
    }
}
