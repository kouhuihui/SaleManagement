namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGoldPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpotGoodsOrders", "GoldPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpotGoodsOrders", "GoldPrice");
        }
    }
}
