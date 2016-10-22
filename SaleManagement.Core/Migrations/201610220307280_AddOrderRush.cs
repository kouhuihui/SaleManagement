namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderRush : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderRushStatus", c => c.Int(nullable: false));
            AddColumn("dbo.ShipmentOrderInfoes", "RushCost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentOrderInfoes", "RushCost");
            DropColumn("dbo.Orders", "OrderRushStatus");
        }
    }
}
