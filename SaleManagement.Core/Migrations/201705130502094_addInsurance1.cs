namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInsurance1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "InsuranceFee");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "InsuranceFee", c => c.Double(nullable: false));
        }
    }
}
