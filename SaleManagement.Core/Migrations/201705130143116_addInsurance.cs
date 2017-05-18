namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInsurance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsInsure", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Insurance", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "RiskType", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "InsuranceFee", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "InsuranceFee");
            DropColumn("dbo.Orders", "RiskType");
            DropColumn("dbo.Orders", "Insurance");
            DropColumn("dbo.Orders", "IsInsure");
        }
    }
}
