namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSpotGoodsPattern : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SpotGoods", "ColorFormId", "dbo.ColorForms");
            DropForeignKey("dbo.SpotGoodsSetStoneInfoes", "SpotGoodsId", "dbo.SpotGoods");
            DropForeignKey("dbo.SpotGoodsAttachments", "SpotGoodsId", "dbo.SpotGoods");
            DropIndex("dbo.SpotGoods", new[] { "ColorFormId" });
            DropIndex("dbo.SpotGoodsSetStoneInfoes", new[] { "SpotGoodsId" });
            DropIndex("dbo.SpotGoodsAttachments", new[] { "SpotGoodsId" });
            CreateTable(
                "dbo.SpotGoodsPatterns",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        Type = c.Int(nullable: false),
                        FileInfoId = c.String(nullable: false, maxLength: 36),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.SpotGoods");
            DropTable("dbo.SpotGoodsSetStoneInfoes");
            DropTable("dbo.SpotGoodsAttachments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SpotGoodsAttachments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        SpotGoodsId = c.String(nullable: false, maxLength: 36),
                        FileInfoId = c.String(nullable: false, maxLength: 36),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(nullable: false, maxLength: 36),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpotGoodsSetStoneInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MatchStoneId = c.Int(nullable: false),
                        MathchStoneName = c.String(),
                        Price = c.Double(nullable: false),
                        WorkingCost = c.Int(nullable: false),
                        SpotGoodsId = c.String(nullable: false, maxLength: 36),
                        Number = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpotGoods",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        Name = c.String(nullable: false, maxLength: 50),
                        SpotGoodsType = c.Int(nullable: false),
                        ColorFormId = c.Int(nullable: false),
                        HandSize = c.Int(nullable: false),
                        MainStone = c.String(),
                        Weight = c.Double(nullable: false),
                        GoldWeight = c.Double(nullable: false),
                        MosaicCost = c.Double(nullable: false),
                        IsMosaic = c.Boolean(nullable: false),
                        CreatorId = c.String(),
                        Created = c.DateTime(nullable: false),
                        IsLock = c.Boolean(nullable: false),
                        status = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.SpotGoodsPatterns");
            CreateIndex("dbo.SpotGoodsAttachments", "SpotGoodsId");
            CreateIndex("dbo.SpotGoodsSetStoneInfoes", "SpotGoodsId");
            CreateIndex("dbo.SpotGoods", "ColorFormId");
            AddForeignKey("dbo.SpotGoodsAttachments", "SpotGoodsId", "dbo.SpotGoods", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SpotGoodsSetStoneInfoes", "SpotGoodsId", "dbo.SpotGoods", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SpotGoods", "ColorFormId", "dbo.ColorForms", "Id", cascadeDelete: true);
        }
    }
}
