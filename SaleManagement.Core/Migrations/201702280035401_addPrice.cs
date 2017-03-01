namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpotGoods", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpotGoods", "Price");
        }
    }
}
