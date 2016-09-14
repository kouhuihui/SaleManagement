using SaleManagement.Core.Models;
using System.Data.Entity;

namespace SaleManagement.Core.Store
{
    public class SaleManagementDbContext : DbContext
    {
        public SaleManagementDbContext() : this(null)
        {
        }

        public SaleManagementDbContext(string nameOrConnectionString) : base(nameOrConnectionString ?? "SaleManagementDbContext")
        {
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public IDbSet<SaleUser> SaleUsers { get; set; }

        public IDbSet<ColorForm> ColorForms { get; set; }

        public IDbSet<Company> Companys { get; set; }

        public IDbSet<CustomerDiscountRate> CustomerDiscountRates { get; set; }

        public IDbSet<CustomerInfo> CustomerInfos { get; set; }

        public IDbSet<DailyGoldPrice> DailyGoldPrices { get; set; }

        public IDbSet<Department> Departments { get; set; }

        public IDbSet<FileInfo> FileInfos { get; set; }

        public IDbSet<GemCategory> GemCategorys { get; set; }

        public IDbSet<MatchStone> MatchStones { get; set; }

        public IDbSet<OrderOperationLog> OrderOperationLogs { get; set; }

        public IDbSet<OrderSetStoneInfo> OrderSetStoneInfos { get; set; }

        public IDbSet<ProductCategory> ProductCategorys { get; set; }

        public IDbSet<Reconciliation> Reconciliations { get; set; }

        public IDbSet<Role> Roles { get; set; }

        public IDbSet<RoleMenu> RoleMenus { get; set; }

        public IDbSet<SerialNumber> SerialNumbers { get; set; }

        public IDbSet<ShipmentOrder> ShipmentOrders { get; set; }

        public IDbSet<ShipmentOrderInfo> ShipmentOrderInfos { get; set; }

        public IDbSet<SystemMenu> Menus { get; set; }

        public IDbSet<Notice> Notices { get; set; }

        public IDbSet<ShippingScheduleSetting> ShippingScheduleSettings { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
