namespace Contexts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Creationbaseandaddingfirstdatas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Parent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 30, storeType: "nvarchar"),
                        Password = c.String(nullable: false, unicode: false),
                        Email = c.String(nullable: false, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ModifiedAt = c.DateTime(nullable: false, precision: 0),
                        Status_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.Status_Id, cascadeDelete: true)
                .Index(t => t.Status_Id);
            
            CreateTable(
                "dbo.Phone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false, unicode: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ModifiedAt = c.DateTime(nullable: false, precision: 0),
                        Parent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parent", t => t.Parent_Id)
                .Index(t => t.Parent_Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ModifiedAt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BirthDate = c.DateTime(nullable: false, precision: 0),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ModifiedAt = c.DateTime(nullable: false, precision: 0),
                        Period_Id = c.Int(nullable: false),
                        Parent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Period", t => t.Period_Id, cascadeDelete: true)
                .ForeignKey("dbo.Parent", t => t.Parent_Id)
                .Index(t => t.Period_Id)
                .Index(t => t.Parent_Id);
            
            CreateTable(
                "dbo.Period",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ModifiedAt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ModifiedAt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, unicode: false),
                        UserName = c.String(nullable: false, unicode: false),
                        Password = c.String(nullable: false, unicode: false),
                        Email = c.String(nullable: false, unicode: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ModifiedAt = c.DateTime(nullable: false, precision: 0),
                        Status_Id = c.Int(nullable: false),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.Status_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserType", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Status_Id)
                .Index(t => t.Type_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "Type_Id", "dbo.UserType");
            DropForeignKey("dbo.User", "Status_Id", "dbo.Status");
            DropForeignKey("dbo.Student", "Parent_Id", "dbo.Parent");
            DropForeignKey("dbo.Student", "Period_Id", "dbo.Period");
            DropForeignKey("dbo.Parent", "Status_Id", "dbo.Status");
            DropForeignKey("dbo.Phone", "Parent_Id", "dbo.Parent");
            DropIndex("dbo.User", new[] { "Type_Id" });
            DropIndex("dbo.User", new[] { "Status_Id" });
            DropIndex("dbo.Student", new[] { "Parent_Id" });
            DropIndex("dbo.Student", new[] { "Period_Id" });
            DropIndex("dbo.Phone", new[] { "Parent_Id" });
            DropIndex("dbo.Parent", new[] { "Status_Id" });
            DropTable("dbo.User");
            DropTable("dbo.UserType");
            DropTable("dbo.Period");
            DropTable("dbo.Student");
            DropTable("dbo.Status");
            DropTable("dbo.Phone");
            DropTable("dbo.Parent");
        }
    }
}
