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
    public class ShipmentManager : BaseManager
    {
        public ShipmentManager()
        {

        }

        public ShipmentManager(SaleUser saleUser) : base(saleUser)
        {

        }

        public async Task<Paging<ShipmentOrder>> GetShipmentOrdersAsync(int start, int take, Func<IQueryable<ShipmentOrder>, IQueryable<ShipmentOrder>> filter = null)
        {
            var query = DbContext.Set<ShipmentOrder>().Where(o => o.CreatorId != null);
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.DeliveryDate).Skip(start).Take(take).ToListAsync();

            return new Paging<ShipmentOrder>(start, take, total, list);
        }

        public async Task<ShipmentOrder> GetShipmentOrderAsync(string id)
        {
            return await DbContext.Set<ShipmentOrder>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<InvokedResult> UpdateAsync(ShipmentOrder ShipmentOrder)
        {
            DbContext.Set<ShipmentOrder>().AddOrUpdate(ShipmentOrder);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> CreateAsync(ShipmentOrder ShipmentOrder)
        {
            ShipmentOrder.CreatorId = User.Id;
            ShipmentOrder.CreatorName = User.Name;
            ShipmentOrder.Created = DateTime.Now;
            DbContext.Set<ShipmentOrder>().AddOrUpdate(ShipmentOrder);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }
    }
}
