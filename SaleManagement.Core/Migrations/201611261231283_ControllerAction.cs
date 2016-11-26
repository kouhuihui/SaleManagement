namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControllerAction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemMenus", "ControllerAction", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemMenus", "ControllerAction");
        }
    }
}
