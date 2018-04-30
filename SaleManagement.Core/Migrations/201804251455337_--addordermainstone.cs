namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addordermainstone : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderMainStoneInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MainStoneId = c.Int(nullable: false),
                        OrderId = c.String(nullable: false, maxLength: 36),
                        Weight = c.Double(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MainStones", t => t.MainStoneId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.MainStoneId)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderMainStoneInfoes", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderMainStoneInfoes", "MainStoneId", "dbo.MainStones");
            DropIndex("dbo.OrderMainStoneInfoes", new[] { "OrderId" });
            DropIndex("dbo.OrderMainStoneInfoes", new[] { "MainStoneId" });
            DropTable("dbo.OrderMainStoneInfoes");
        }
    }
}
