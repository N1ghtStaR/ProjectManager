namespace ProjectManagerDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Messages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeveloperID = c.Int(nullable: false),
                        Username = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Developers", t => t.DeveloperID, cascadeDelete: true)
                .Index(t => t.DeveloperID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "DeveloperID", "dbo.Developers");
            DropIndex("dbo.Messages", new[] { "DeveloperID" });
            DropTable("dbo.Messages");
        }
    }
}
