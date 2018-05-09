namespace ProjectManagerDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Developers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 32),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 64),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Incomes",
                c => new
                    {
                        ProjectID = c.Int(nullable: false),
                        DeveleperID = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Amount = c.Double(nullable: false),
                        ReleaseDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.Developers", t => t.DeveleperID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .Index(t => t.ProjectID)
                .Index(t => t.DeveleperID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeveleperID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 32),
                        Description = c.String(nullable: false, maxLength: 256),
                        Category = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Developers", t => t.DeveleperID, cascadeDelete: true)
                .Index(t => t.DeveleperID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 256),
                        Priority = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Incomes", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Tasks", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Projects", "DeveleperID", "dbo.Developers");
            DropForeignKey("dbo.Incomes", "DeveleperID", "dbo.Developers");
            DropIndex("dbo.Tasks", new[] { "ProjectID" });
            DropIndex("dbo.Projects", new[] { "DeveleperID" });
            DropIndex("dbo.Incomes", new[] { "DeveleperID" });
            DropIndex("dbo.Incomes", new[] { "ProjectID" });
            DropTable("dbo.Tasks");
            DropTable("dbo.Projects");
            DropTable("dbo.Incomes");
            DropTable("dbo.Developers");
        }
    }
}
