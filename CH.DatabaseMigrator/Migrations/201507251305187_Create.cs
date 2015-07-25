namespace CH.DatabaseMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
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
                        Species = c.String(maxLength: 20),
                        BreedName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Species);
            
            CreateTable(
                "dbo.Species",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 50),
                        Singular = c.String(maxLength: 50),
                        Plural = c.String(maxLength: 50),
                        YoungSingular = c.String(maxLength: 50),
                        YoungPlural = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Name);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Breed", new[] { "Species" });
            DropIndex("dbo.AnimalStatus", new[] { "Name" });
            DropTable("dbo.Species");
            DropTable("dbo.Breed");
            DropTable("dbo.AnimalStatus");
        }
    }
}
