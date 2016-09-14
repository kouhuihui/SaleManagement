namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShippingScheduleSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShippingScheduleSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Days = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        UserId = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShippingScheduleSettings");
        }
    }
}
