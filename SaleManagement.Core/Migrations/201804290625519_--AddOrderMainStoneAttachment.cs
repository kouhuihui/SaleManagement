namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderMainStoneAttachment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderMainStoneAttachments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        OrderMainStoneInfoId = c.String(nullable: false, maxLength: 36),
                        FileInfoId = c.String(nullable: false, maxLength: 36),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(nullable: false, maxLength: 36),
                        OrderMainStoneInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderMainStoneInfoes", t => t.OrderMainStoneInfo_Id)
                .Index(t => t.OrderMainStoneInfo_Id);
            
            AddColumn("dbo.OrderMainStoneInfoes", "Remark", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderMainStoneAttachments", "OrderMainStoneInfo_Id", "dbo.OrderMainStoneInfoes");
            DropIndex("dbo.OrderMainStoneAttachments", new[] { "OrderMainStoneInfo_Id" });
            DropColumn("dbo.OrderMainStoneInfoes", "Remark");
            DropTable("dbo.OrderMainStoneAttachments");
        }
    }
}
