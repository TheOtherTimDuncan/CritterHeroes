namespace CH.DatabaseMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Locations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Location",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 50),
                    RescueGroupsID = c.String(maxLength: 8, unicode: false),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.RescueGroupsID);

            AddColumn("dbo.Critter", "LocationID", c => c.Int());
            CreateIndex("dbo.Critter", "LocationID");
            AddForeignKey("dbo.Critter", "LocationID", "dbo.Location", "ID");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Critter", "LocationID", "dbo.Location");
            DropIndex("dbo.Location", new[] { "RescueGroupsID" });
            DropIndex("dbo.Critter", new[] { "LocationID" });
            DropColumn("dbo.Critter", "LocationID");
            DropTable("dbo.Location");
        }
    }
}
