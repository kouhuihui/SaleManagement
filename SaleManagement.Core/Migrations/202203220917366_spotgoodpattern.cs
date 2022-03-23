namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spotgoodpattern : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpotGoodsPatterns", "ProductCategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.SpotGoodsPatterns", "GemCategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.SpotGoodsPatterns", "Price", c => c.Double(nullable: false));
            AddColumn("dbo.SpotGoodsPatterns", "ReferenceData", c => c.String());
            AddColumn("dbo.SpotGoodsPatterns", "RowNo", c => c.Int(nullable: false));
            CreateIndex("dbo.SpotGoodsPatterns", "ProductCategoryId");
            CreateIndex("dbo.SpotGoodsPatterns", "GemCategoryId");
            AddForeignKey("dbo.SpotGoodsPatterns", "GemCategoryId", "dbo.GemCategories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SpotGoodsPatterns", "ProductCategoryId", "dbo.ProductCategories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpotGoodsPatterns", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.SpotGoodsPatterns", "GemCategoryId", "dbo.GemCategories");
            DropIndex("dbo.SpotGoodsPatterns", new[] { "GemCategoryId" });
            DropIndex("dbo.SpotGoodsPatterns", new[] { "ProductCategoryId" });
            DropColumn("dbo.SpotGoodsPatterns", "RowNo");
            DropColumn("dbo.SpotGoodsPatterns", "ReferenceData");
            DropColumn("dbo.SpotGoodsPatterns", "Price");
            DropColumn("dbo.SpotGoodsPatterns", "GemCategoryId");
            DropColumn("dbo.SpotGoodsPatterns", "ProductCategoryId");
        }
    }
}
