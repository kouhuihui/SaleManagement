using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class DiscountRateManager : BaseManager
    {
        public DiscountRateManager() : base()
        {
        }

        public async Task<CustomerDiscountRate> GetCustomerDiscountRateAsync(string customerId)
        {
            var discountRate = await DbContext.Set<CustomerDiscountRate>().FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (discountRate == null)
                return new CustomerDiscountRate();

            return discountRate;
        }

        public async Task<IEnumerable<CustomerDiscountRate>> GetCustomerDiscountRatesAsync(IEnumerable<string> customerIds)
        {
            if (customerIds == null || !customerIds.Any())
                return Enumerable.Empty<CustomerDiscountRate>();

            return await DbContext.Set<CustomerDiscountRate>().Where(c => customerIds.Contains(c.CustomerId)).ToListAsync();
        }

        public async Task<InvokedResult> SaveDiscountRateAsync(CustomerDiscountRate discountRate)
        {
            DbContext.Set<CustomerDiscountRate>().AddOrUpdate(discountRate);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }
    }
}
