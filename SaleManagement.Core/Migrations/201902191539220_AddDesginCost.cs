namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDesginCost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DesginCost", c => c.Double(nullable: false));
            AddColumn("dbo.ShipmentOrderInfoes", "DesginCost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentOrderInfoes", "DesginCost");
            DropColumn("dbo.Orders", "DesginCost");
        }
    }
}
