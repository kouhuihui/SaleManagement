namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDesginAuditTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DesginAuditTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "DesginAuditTime");
        }
    }
}
