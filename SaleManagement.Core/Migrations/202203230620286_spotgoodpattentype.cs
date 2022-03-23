namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spotgoodpattentype : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SpotGoodsPatterns", "SpotGoodTypeId", "dbo.SpotGoodTypes");
            DropIndex("dbo.SpotGoodsPatterns", new[] { "SpotGoodTypeId" });
            CreateTable(
                "dbo.SpotGoodsPatternTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SpotGoodsPatternId = c.String(nullable: false, maxLength: 128),
                        SpotGoodsTypeId = c.String(nullable: false, maxLength: 36),
                        SpotGoodsType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SpotGoodsPatterns", t => t.SpotGoodsPatternId, cascadeDelete: true)
                .Index(t => t.SpotGoodsPatternId);
            
            DropColumn("dbo.SpotGoodsPatterns", "SpotGoodTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpotGoodsPatterns", "SpotGoodTypeId", c => c.String(nullable: false, maxLength: 36));
            DropForeignKey("dbo.SpotGoodsPatternTypes", "SpotGoodsPatternId", "dbo.SpotGoodsPatterns");
            DropIndex("dbo.SpotGoodsPatternTypes", new[] { "SpotGoodsPatternId" });
            DropTable("dbo.SpotGoodsPatternTypes");
            CreateIndex("dbo.SpotGoodsPatterns", "SpotGoodTypeId");
            AddForeignKey("dbo.SpotGoodsPatterns", "SpotGoodTypeId", "dbo.SpotGoodTypes", "Id", cascadeDelete: true);
        }
    }
}
