using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
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
    }
}
