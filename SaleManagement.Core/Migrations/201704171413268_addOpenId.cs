namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOpenId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpotGoodsOrders", "OpenId", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpotGoodsOrders", "OpenId");
        }
    }
}
