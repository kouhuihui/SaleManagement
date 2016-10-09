using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class OrderSetStoneInfoManager : BaseManager
    {
        public OrderSetStoneInfoManager()
        {

        }

        public OrderSetStoneInfoManager(SaleUser user) : base()
        {

        }

        public async Task<InvokedResult> CreateOrderSetStoneInfoAsync(OrderSetStoneInfo info)
        {
            DbContext.Set<OrderSetStoneInfo>().AddOrUpdate(info);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> DeleteOrderSetStoneAsync(int id)
        {
            var orderSetStoneInfo = DbContext.Set<OrderSetStoneInfo>().FirstOrDefault(r => r.Id == id);
            if (orderSetStoneInfo == null)
                return InvokedResult.Fail("404", "配石不存在");

            DbContext.Set<OrderSetStoneInfo>().Remove(orderSetStoneInfo);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }
    }
}
