namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWxbinding : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountBindings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UserName = c.String(),
                        WxAccount = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccountBindings");
        }
    }
}
