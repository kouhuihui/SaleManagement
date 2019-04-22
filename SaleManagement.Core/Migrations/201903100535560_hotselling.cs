namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotselling : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HotSellingAttachments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        HotSellingId = c.String(nullable: false, maxLength: 36),
                        FileInfoId = c.String(nullable: false, maxLength: 36),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(nullable: false, maxLength: 36),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotSellings", t => t.HotSellingId, cascadeDelete: true)
                .Index(t => t.HotSellingId);
            
            CreateTable(
                "dbo.HotSellings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ColorFormId = c.Int(nullable: false),
                        ProductCategoryId = c.Int(nullable: false),
                        GemCategoryId = c.Int(nullable: false),
                        VersionNo = c.String(nullable: false, maxLength: 36),
                        RowNo = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ColorForms", t => t.ColorFormId, cascadeDelete: true)
                .ForeignKey("dbo.GemCategories", t => t.GemCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryId, cascadeDelete: true)
                .Index(t => t.ColorFormId)
                .Index(t => t.ProductCategoryId)
                .Index(t => t.GemCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HotSellings", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.HotSellings", "GemCategoryId", "dbo.GemCategories");
            DropForeignKey("dbo.HotSellings", "ColorFormId", "dbo.ColorForms");
            DropForeignKey("dbo.HotSellingAttachments", "HotSellingId", "dbo.HotSellings");
            DropIndex("dbo.HotSellings", new[] { "GemCategoryId" });
            DropIndex("dbo.HotSellings", new[] { "ProductCategoryId" });
            DropIndex("dbo.HotSellings", new[] { "ColorFormId" });
            DropIndex("dbo.HotSellingAttachments", new[] { "HotSellingId" });
            DropTable("dbo.HotSellings");
            DropTable("dbo.HotSellingAttachments");
        }
    }
}
