namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomerAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerAddresses",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        OpenId = c.String(maxLength: 256),
                        Address = c.String(maxLength: 256),
                        Name = c.String(maxLength: 256),
                        Phone = c.String(maxLength: 11),
                        Created = c.DateTime(nullable: false),
                        IsCommonly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpotGoodsOrders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        SpotGoodsId = c.String(maxLength: 36),
                        ProductNo = c.String(nullable: false, maxLength: 256),
                        IsMosaic = c.Boolean(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Created = c.DateTime(nullable: false),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SpotGoods", t => t.SpotGoodsId)
                .Index(t => t.SpotGoodsId);
            
            DropColumn("dbo.SpotGoods", "IsMosaic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpotGoods", "IsMosaic", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.SpotGoodsOrders", "SpotGoodsId", "dbo.SpotGoods");
            DropIndex("dbo.SpotGoodsOrders", new[] { "SpotGoodsId" });
            DropTable("dbo.SpotGoodsOrders");
            DropTable("dbo.CustomerAddresses");
        }
    }
}
