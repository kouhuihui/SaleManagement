namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentAddCompanyId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentOrders", "CompanyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentOrders", "CompanyId");
        }
    }
}
