namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RepairOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RepairOrders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ShipmentOrderId = c.String(nullable: false, maxLength: 36),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        GoldWeight = c.Double(nullable: false),
                        GoldAmount = c.Double(nullable: false),
                        StoneNumber = c.Int(nullable: false),
                        StoneWeight = c.Double(nullable: false),
                        StoneAmount = c.Double(nullable: false),
                        Remark = c.String(),
                        TotalAmount = c.Double(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShipmentOrders", t => t.ShipmentOrderId, cascadeDelete: true)
                .Index(t => t.ShipmentOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RepairOrders", "ShipmentOrderId", "dbo.ShipmentOrders");
            DropIndex("dbo.RepairOrders", new[] { "ShipmentOrderId" });
            DropTable("dbo.RepairOrders");
        }
    }
}
