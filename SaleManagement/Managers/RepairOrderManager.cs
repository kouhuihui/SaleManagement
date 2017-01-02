using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class RepairOrderManager : BaseManager
    {
        public RepairOrderManager(SaleUser user) : base(user)
        {

        }

        public async Task<InvokedResult> CreateRepairOrder(RepairOrder repairOrder)
        {
            DbContext.Set<RepairOrder>().Add(repairOrder);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> DeleteAsync(RepairOrder repairOrder)
        {
            DbContext.Set<RepairOrder>().Remove(repairOrder);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<RepairOrder> GetRepairOrder(string id)
        {
            Requires.NotNullOrEmpty(id, "id");
            return await DbContext.Set<RepairOrder>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<RepairOrder>> GetRepairOrders(string shipmentOrderId)
        {
            Requires.NotNullOrEmpty(shipmentOrderId, "shipmentOrderId");

            return await DbContext.Set<RepairOrder>().Where(r => r.ShipmentOrderId == shipmentOrderId).ToListAsync();
        }
    }
}
