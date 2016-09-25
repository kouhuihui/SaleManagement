using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class CustomerInfoManager : BaseManager
    {
        public CustomerInfoManager(SaleUser user) : base(user)
        {

        }

        public async Task<CustomerInfo> GetCustomerInfoAsync(string customerId)
        {
            return await DbContext.Set<CustomerInfo>().FirstOrDefaultAsync(c => c.UserId == customerId);
        }

        public async Task<IEnumerable<CustomerInfo>> GetCustomerInfosRatesAsync(IEnumerable<string> customerIds)
        {
            if (customerIds == null || !customerIds.Any())
                return Enumerable.Empty<CustomerInfo>();

            return await DbContext.Set<CustomerInfo>().Where(c => customerIds.Contains(c.UserId)).ToListAsync();
        }


        public async Task<InvokedResult> SaveCustomerInfoAsync(CustomerInfo customerInfo)
        {
            DbContext.Set<CustomerInfo>().AddOrUpdate(customerInfo);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }
    }
}
