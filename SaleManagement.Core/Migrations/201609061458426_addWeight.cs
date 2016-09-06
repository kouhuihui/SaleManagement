namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWeight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Weight", c => c.Double(nullable: false));
            AddColumn("dbo.Orders", "GoldWeight", c => c.Double(nullable: false));
            DropColumn("dbo.Orders", "WaxCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "WaxCost", c => c.Double(nullable: false));
            DropColumn("dbo.Orders", "GoldWeight");
            DropColumn("dbo.Orders", "Weight");
        }
    }
}
