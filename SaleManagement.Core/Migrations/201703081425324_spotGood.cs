namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spotGood : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ColorForms", t => t.ColorFormId, cascadeDelete: true)
                .Index(t => t.ColorFormId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SpotGoods", t => t.SpotGoodsId, cascadeDelete: true)
                .Index(t => t.SpotGoodsId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SpotGoods", t => t.SpotGoodsId, cascadeDelete: true)
                .Index(t => t.SpotGoodsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpotGoodsSetStoneInfoes", "SpotGoodsId", "dbo.SpotGoods");
            DropForeignKey("dbo.SpotGoods", "ColorFormId", "dbo.ColorForms");
            DropForeignKey("dbo.SpotGoodsAttachments", "SpotGoodsId", "dbo.SpotGoods");
            DropIndex("dbo.SpotGoodsSetStoneInfoes", new[] { "SpotGoodsId" });
            DropIndex("dbo.SpotGoodsAttachments", new[] { "SpotGoodsId" });
            DropIndex("dbo.SpotGoods", new[] { "ColorFormId" });
            DropTable("dbo.SpotGoodsSetStoneInfoes");
            DropTable("dbo.SpotGoodsAttachments");
            DropTable("dbo.SpotGoods");
        }
    }
}
