namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatesf : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpotGoodsOrders", "SfNo", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SpotGoodsOrders", "SfNo", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
