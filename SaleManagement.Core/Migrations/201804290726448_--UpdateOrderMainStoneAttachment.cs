namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderMainStoneAttachment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderMainStoneAttachments", "OrderMainStoneInfo_Id", "dbo.OrderMainStoneInfoes");
            DropIndex("dbo.OrderMainStoneAttachments", new[] { "OrderMainStoneInfo_Id" });
            DropColumn("dbo.OrderMainStoneAttachments", "OrderMainStoneInfoId");
            RenameColumn(table: "dbo.OrderMainStoneAttachments", name: "OrderMainStoneInfo_Id", newName: "OrderMainStoneInfoId");
            AlterColumn("dbo.OrderMainStoneAttachments", "OrderMainStoneInfoId", c => c.Int(nullable: false));
            AlterColumn("dbo.OrderMainStoneAttachments", "OrderMainStoneInfoId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderMainStoneAttachments", "OrderMainStoneInfoId");
            AddForeignKey("dbo.OrderMainStoneAttachments", "OrderMainStoneInfoId", "dbo.OrderMainStoneInfoes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderMainStoneAttachments", "OrderMainStoneInfoId", "dbo.OrderMainStoneInfoes");
            DropIndex("dbo.OrderMainStoneAttachments", new[] { "OrderMainStoneInfoId" });
            AlterColumn("dbo.OrderMainStoneAttachments", "OrderMainStoneInfoId", c => c.Int());
            AlterColumn("dbo.OrderMainStoneAttachments", "OrderMainStoneInfoId", c => c.String(nullable: false, maxLength: 36));
            RenameColumn(table: "dbo.OrderMainStoneAttachments", name: "OrderMainStoneInfoId", newName: "OrderMainStoneInfo_Id");
            AddColumn("dbo.OrderMainStoneAttachments", "OrderMainStoneInfoId", c => c.String(nullable: false, maxLength: 36));
            CreateIndex("dbo.OrderMainStoneAttachments", "OrderMainStoneInfo_Id");
            AddForeignKey("dbo.OrderMainStoneAttachments", "OrderMainStoneInfo_Id", "dbo.OrderMainStoneInfoes", "Id");
        }
    }
}
