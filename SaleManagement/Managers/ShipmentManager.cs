using Dickson.Core.ComponentModel;
using EntityFramework.Extensions;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
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
            var query = DbContext.Set<ShipmentOrder>().AsQueryable();
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

        public async Task<IEnumerable<ShipmentOrderInfo>> GetShipmentOrderInfosAsync(Func<IQueryable<ShipmentOrderInfo>, IQueryable<ShipmentOrderInfo>> filter = null)
        {
            var query = DbContext.Set<ShipmentOrderInfo>().Where(r => r.Order.ComplayId == User.CompanyId);
            if (filter != null)
            {
                query = filter(query);
            }

            return await query.OrderByDescending(u => u.ShipmentOrder.Created).ToListAsync();
        }

        public async Task<ShipmentOrder> GetShipmentOrderByOrderIdAsync(string orderId)
        {
            var shipmentInfo = await DbContext.Set<ShipmentOrderInfo>().FirstOrDefaultAsync(r => r.Id == orderId);
            return shipmentInfo.ShipmentOrder;
        }

        public async Task<InvokedResult> UpdateAsync(ShipmentOrder shipmentOrder)
        {
            shipmentOrder.CreatorId = User.Id;
            shipmentOrder.CreatorName = User.Name;
            DbContext.Set<ShipmentOrder>().AddOrUpdate(shipmentOrder);
            await UpdateShipmentOrderInfosAsync(shipmentOrder);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> AuditShipmentOrder(ShipmentOrder shipmentOrder)
        {
            DbContext.Set<ShipmentOrder>().AddOrUpdate(shipmentOrder);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> CreateAsync(ShipmentOrder shipmentOrder)
        {
            shipmentOrder.CreatorId = User.Id;
            shipmentOrder.CreatorName = User.Name;
            shipmentOrder.Created = DateTime.Now;
            DbContext.Set<ShipmentOrder>().AddOrUpdate(shipmentOrder);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        private async Task UpdateShipmentOrderInfosAsync(ShipmentOrder order)
        {
            var shipmentOrderInfos = DbContext.Set<ShipmentOrderInfo>();
            await shipmentOrderInfos.Where(a => a.ShipmentOrderId == order.Id).DeleteAsync();
            shipmentOrderInfos.AddRange(order.ShipmentOrderInfos);
        }
    }
}
