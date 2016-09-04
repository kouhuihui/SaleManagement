namespace SaleManagement.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ColorForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Purity = c.String(nullable: false, maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        ShortName = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(maxLength: 24),
                        Address = c.String(maxLength: 256),
                        ContactPerson = c.String(nullable: false, maxLength: 20),
                        Remark = c.String(maxLength: 256),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerDiscountRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(nullable: false, maxLength: 36),
                        StoneSetter = c.Int(nullable: false),
                        SideStone = c.Int(nullable: false),
                        PriceOfWork = c.Int(nullable: false),
                        Loss18K = c.Int(nullable: false),
                        LossPt = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaleUsers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.SaleUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        Name = c.String(nullable: false, maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Telephone = c.String(maxLength: 24),
                        Mobile = c.String(maxLength: 11),
                        Email = c.String(maxLength: 50),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(nullable: false, maxLength: 128),
                        SecurityStamp = c.String(nullable: false, maxLength: 128),
                        LockoutEndDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.Int(nullable: false),
                        IdentityType = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Code = c.String(nullable: false, maxLength: 50),
                        Type = c.Int(nullable: false),
                        Description = c.String(maxLength: 256),
                        Deleted = c.Boolean(nullable: false),
                        IsSystemRole = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerInfoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        Address = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DailyGoldPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        ColorFormId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        CreatorId = c.String(nullable: false, maxLength: 36),
                        Created = c.DateTime(nullable: false),
                        UpdaterId = c.String(maxLength: 36),
                        Updated = c.DateTime(nullable: false),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ColorForms", t => t.ColorFormId, cascadeDelete: true)
                .Index(t => t.ColorFormId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ParentId = c.Int(),
                        order = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FileInfoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        FileName = c.String(nullable: false, maxLength: 50),
                        ContentLength = c.Int(nullable: false),
                        ContentType = c.String(nullable: false, maxLength: 128),
                        Expiration = c.DateTime(nullable: false),
                        Data = c.Binary(nullable: false),
                        ThumbnailData = c.Binary(),
                        Created = c.DateTime(nullable: false),
                        Purpose = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GemCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MatchStones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Price = c.Double(nullable: false),
                        WorkingCost = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SystemMenus",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Level = c.Int(nullable: false),
                        parentId = c.Int(),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 256),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderOperationLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatorId = c.String(),
                        CreatorName = c.String(),
                        Created = c.DateTime(nullable: false),
                        Content = c.String(),
                        Status = c.Int(nullable: false),
                        OrderId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderSetStoneInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MatchStoneId = c.Int(nullable: false),
                        MathchStoneName = c.String(),
                        Price = c.Double(nullable: false),
                        WorkingCost = c.Int(nullable: false),
                        OrderId = c.String(nullable: false, maxLength: 36),
                        Number = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ColorFormId = c.Int(nullable: false),
                        ProductCategoryId = c.Int(nullable: false),
                        GemCategoryId = c.Int(nullable: false),
                        HandSize = c.Double(nullable: false),
                        MinChainLength = c.Double(nullable: false),
                        MaxChainLength = c.Double(nullable: false),
                        Number = c.Int(nullable: false),
                        MainStoneNumber = c.Int(nullable: false),
                        MainStoneSize = c.Double(nullable: false),
                        Certificate = c.String(maxLength: 10),
                        WordsPrinted = c.String(maxLength: 50),
                        RabbetRequirement = c.Int(nullable: false),
                        StoneDescribe = c.Int(nullable: false),
                        GoldWeightRequirement = c.Int(nullable: false),
                        SideStoneRequiredment = c.Int(nullable: false),
                        RadianRequirement = c.Int(nullable: false),
                        HasOldMaterial = c.Boolean(nullable: false),
                        Remark = c.String(maxLength: 256),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(nullable: false, maxLength: 36),
                        CreatorName = c.String(nullable: false, maxLength: 50),
                        Updated = c.DateTime(nullable: false),
                        ComplayId = c.Int(nullable: false),
                        CustomerId = c.String(nullable: false, maxLength: 36),
                        OrderStatus = c.Int(nullable: false),
                        ModuleType = c.Int(nullable: false),
                        CurrentUserId = c.String(maxLength: 36),
                        DeliveryDate = c.DateTime(),
                        OutputWaxCost = c.Double(nullable: false),
                        WaxCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ColorForms", t => t.ColorFormId, cascadeDelete: true)
                .ForeignKey("dbo.SaleUsers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.GemCategories", t => t.GemCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryId, cascadeDelete: true)
                .Index(t => t.ColorFormId)
                .Index(t => t.ProductCategoryId)
                .Index(t => t.GemCategoryId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.OrderAttachments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        OrderId = c.String(nullable: false, maxLength: 36),
                        FileInfoId = c.String(nullable: false, maxLength: 36),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.String(nullable: false, maxLength: 36),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reconciliations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        CustomerId = c.String(nullable: false, maxLength: 36),
                        CustomerName = c.String(nullable: false, maxLength: 50),
                        CreatorId = c.String(nullable: false, maxLength: 36),
                        CompanyId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        SystemMenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.SystemMenus", t => t.SystemMenuId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.SystemMenuId);
            
            CreateTable(
                "dbo.SerialNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SN = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShipmentOrderInfoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ShipmentOrderId = c.String(nullable: false, maxLength: 36),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        Weight = c.Double(nullable: false),
                        GoldWeight = c.Double(nullable: false),
                        LossRate = c.Double(nullable: false),
                        GoldPrice = c.Double(nullable: false),
                        GoldAmount = c.Double(nullable: false),
                        RiskFee = c.Double(nullable: false),
                        SideStoneNumber = c.Int(nullable: false),
                        SideStoneWeight = c.Double(nullable: false),
                        TotalSetStoneWorkingCost = c.Double(nullable: false),
                        SideStoneTotalAmount = c.Double(nullable: false),
                        BasicCost = c.Double(nullable: false),
                        OutputWaxCost = c.Double(nullable: false),
                        OtherCost = c.Double(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Id)
                .ForeignKey("dbo.ShipmentOrders", t => t.ShipmentOrderId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ShipmentOrderId);
            
            CreateTable(
                "dbo.ShipmentOrders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        CustomerId = c.String(nullable: false, maxLength: 36),
                        CustomerName = c.String(),
                        DeliveryDate = c.DateTime(nullable: false),
                        TotalNumber = c.Int(nullable: false),
                        TotalWeight = c.Double(nullable: false),
                        TotalGoldWeight = c.Double(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        CreatorId = c.String(),
                        CreatorName = c.String(),
                        Created = c.DateTime(nullable: false),
                        AuditorName = c.String(),
                        AuditDate = c.DateTime(),
                        AuditStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShipmentOrderInfoes", "ShipmentOrderId", "dbo.ShipmentOrders");
            DropForeignKey("dbo.ShipmentOrderInfoes", "Id", "dbo.Orders");
            DropForeignKey("dbo.RoleMenus", "SystemMenuId", "dbo.SystemMenus");
            DropForeignKey("dbo.RoleMenus", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Orders", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.OrderSetStoneInfoes", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "GemCategoryId", "dbo.GemCategories");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.SaleUsers");
            DropForeignKey("dbo.Orders", "ColorFormId", "dbo.ColorForms");
            DropForeignKey("dbo.OrderAttachments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.DailyGoldPrices", "ColorFormId", "dbo.ColorForms");
            DropForeignKey("dbo.CustomerDiscountRates", "CustomerId", "dbo.SaleUsers");
            DropForeignKey("dbo.SaleUsers", "RoleId", "dbo.Roles");
            DropIndex("dbo.ShipmentOrderInfoes", new[] { "ShipmentOrderId" });
            DropIndex("dbo.ShipmentOrderInfoes", new[] { "Id" });
            DropIndex("dbo.RoleMenus", new[] { "SystemMenuId" });
            DropIndex("dbo.RoleMenus", new[] { "RoleId" });
            DropIndex("dbo.OrderAttachments", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.Orders", new[] { "GemCategoryId" });
            DropIndex("dbo.Orders", new[] { "ProductCategoryId" });
            DropIndex("dbo.Orders", new[] { "ColorFormId" });
            DropIndex("dbo.OrderSetStoneInfoes", new[] { "OrderId" });
            DropIndex("dbo.DailyGoldPrices", new[] { "ColorFormId" });
            DropIndex("dbo.SaleUsers", new[] { "RoleId" });
            DropIndex("dbo.CustomerDiscountRates", new[] { "CustomerId" });
            DropTable("dbo.ShipmentOrders");
            DropTable("dbo.ShipmentOrderInfoes");
            DropTable("dbo.SerialNumbers");
            DropTable("dbo.RoleMenus");
            DropTable("dbo.Reconciliations");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.OrderAttachments");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderSetStoneInfoes");
            DropTable("dbo.OrderOperationLogs");
            DropTable("dbo.Notices");
            DropTable("dbo.SystemMenus");
            DropTable("dbo.MatchStones");
            DropTable("dbo.GemCategories");
            DropTable("dbo.FileInfoes");
            DropTable("dbo.Departments");
            DropTable("dbo.DailyGoldPrices");
            DropTable("dbo.CustomerInfoes");
            DropTable("dbo.Roles");
            DropTable("dbo.SaleUsers");
            DropTable("dbo.CustomerDiscountRates");
            DropTable("dbo.Companies");
            DropTable("dbo.ColorForms");
        }
    }
}
