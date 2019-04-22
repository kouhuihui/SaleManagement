namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotsellAddRefer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HotSellings", "ColorFormId", "dbo.ColorForms");
            DropIndex("dbo.HotSellings", new[] { "ColorFormId" });
            AddColumn("dbo.HotSellingAttachments", "FileType", c => c.Int(nullable: false));
            AddColumn("dbo.HotSellings", "Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.HotSellings", "ReferencePrice", c => c.Double(nullable: false));
            AddColumn("dbo.HotSellings", "ReferenceData", c => c.String());
            DropColumn("dbo.HotSellings", "ColorFormId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HotSellings", "ColorFormId", c => c.Int(nullable: false));
            DropColumn("dbo.HotSellings", "ReferenceData");
            DropColumn("dbo.HotSellings", "ReferencePrice");
            DropColumn("dbo.HotSellings", "Name");
            DropColumn("dbo.HotSellingAttachments", "FileType");
            CreateIndex("dbo.HotSellings", "ColorFormId");
            AddForeignKey("dbo.HotSellings", "ColorFormId", "dbo.ColorForms", "Id", cascadeDelete: true);
        }
    }
}
