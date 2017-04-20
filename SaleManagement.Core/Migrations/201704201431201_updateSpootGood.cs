namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSpootGood : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpotGoods", "BasicCost", c => c.Double(nullable: false));
            AddColumn("dbo.SpotGoods", "Loss18KRate", c => c.Int(nullable: false));
            AddColumn("dbo.SpotGoodsOrders", "SfNo", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpotGoodsOrders", "SfNo");
            DropColumn("dbo.SpotGoods", "Loss18KRate");
            DropColumn("dbo.SpotGoods", "BasicCost");
        }
    }
}
