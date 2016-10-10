namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReconciliationRemark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reconciliations", "Remark", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reconciliations", "Remark");
        }
    }
}
