namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMatchStone : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MatchStones", "WorkingCost", c => c.Double(nullable: false));
            AlterColumn("dbo.OrderSetStoneInfoes", "WorkingCost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderSetStoneInfoes", "WorkingCost", c => c.Int(nullable: false));
            AlterColumn("dbo.MatchStones", "WorkingCost", c => c.Int(nullable: false));
        }
    }
}
