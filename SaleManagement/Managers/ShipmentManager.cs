using Dickson.Core.ComponentModel;
using EntityFramework.Extensions;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
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
            var list = await query.OrderByDescending(u => u.Created).Skip(start).Take(take).ToListAsync();

            return new Paging<ShipmentOrder>(start, take, total, list);
        }

        public async Task<ShipmentOrder> GetShipmentOrderAsync(string id)
        {
            return await DbContext.Set<ShipmentOrder>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ICollection<ShipmentOrderInfo>> GetShipmentOrderInfosAsync(Func<IQueryable<ShipmentOrderInfo>, IQueryable<ShipmentOrderInfo>> filter = null)
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

        public async Task<InvokedResult<string>> UpdateAsync(ShipmentOrder shipmentOrder)
        {
            shipmentOrder.CreatorId = User.Id;
            shipmentOrder.CreatorName = User.Name;
            DbContext.Set<ShipmentOrder>().AddOrUpdate(shipmentOrder);
            await UpdateShipmentOrderInfosAsync(shipmentOrder);
            await DbContext.SaveChangesAsync();
            var result = new InvokedResult<string>();
            result.Data = shipmentOrder.Id;
            result.Succeeded = true;

            return result;
        }

        public async Task<InvokedResult> UpdateTotalAmountAsync(string Id, double difference)
        {
            var shipmentOrder = await GetShipmentOrderAsync(Id);
            shipmentOrder.TotalAmount = shipmentOrder.TotalAmount + difference;
            DbContext.Set<ShipmentOrder>().AddOrUpdate(shipmentOrder);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> AuditShipmentOrder(ShipmentOrder shipmentOrder)
        {
            DbContext.Set<ShipmentOrder>().AddOrUpdate(shipmentOrder);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult<string>> CreateAsync(ShipmentOrder shipmentOrder)
        {
            shipmentOrder.CreatorId = User.Id;
            shipmentOrder.CreatorName = User.Name;
            shipmentOrder.Created = DateTime.Now;
            DbContext.Set<ShipmentOrder>().AddOrUpdate(shipmentOrder);
            await DbContext.SaveChangesAsync();
            var result = new InvokedResult<string>();
            result.Data = shipmentOrder.Id;
            result.Succeeded = true;

            return result;
        }

        public async Task<IEnumerable<OrderSetStoneStatistic>> GetOrderSetStoneStatisticsAsync(ReportQueryBaseDto reportQuery)
        {
            var query = DbContext.Set<ShipmentOrderInfo>().Where(t => t.Order.ComplayId == User.CompanyId);
            if (reportQuery.StatisticStartDate.HasValue)
            {
                query = query.Where(t => t.ShipmentOrder.Created > reportQuery.StatisticStartDate.Value);
            }
            if (reportQuery.StatisticEndDate.HasValue)
            {
                var endDate = reportQuery.StatisticEndDate.Value.AddDays(1);
                query = query.Where(t => t.ShipmentOrder.Created < endDate);
            }

            if (!string.IsNullOrEmpty(reportQuery.CustomerId))
            {
                query = query.Where(t => t.ShipmentOrder.CustomerId == reportQuery.CustomerId);
            }

            var orderIds = query.Select(r => r.Order.Id);
            if (!orderIds.Any())
                return Enumerable.Empty<OrderSetStoneStatistic>();

            var orderSetStones = await (from a in DbContext.Set<OrderSetStoneInfo>()
                                        join b in DbContext.Set<Order>()
                                        on a.OrderId equals b.Id
                                        join c in DbContext.Set<CustomerDiscountRate>()
                                        on b.CustomerId equals c.CustomerId
                                        where orderIds.Contains(a.OrderId)
                                        group new { a, c } by new { a.MatchStoneId, a.MathchStoneName })
                                                 .ToListAsync();
            var orderSetStoneStatistics = orderSetStones.Select(g => new OrderSetStoneStatistic
            {
                SetStoneName = g.Key.MathchStoneName,
                Weight = g.Sum(ga => (decimal)ga.a.Weight),
                Number = g.Sum(ga => ga.a.Number),
                SetStoneAmount = g.Sum(ga => ((decimal)(ga.c.SideStone / 100.0 * ga.a.Price * ga.a.Weight)))
            }).ToList();

            var totalWeight = orderSetStoneStatistics.Sum(r => r.Weight);
            var totalNumber = orderSetStoneStatistics.Sum(r => r.Number);
            var setStoneAmount = orderSetStoneStatistics.Sum(r => r.SetStoneAmount);
            orderSetStoneStatistics.Add(new OrderSetStoneStatistic { SetStoneName = "总计", Weight = totalWeight, Number = totalNumber, SetStoneAmount = setStoneAmount });
            return orderSetStoneStatistics;
        }

        private async Task UpdateShipmentOrderInfosAsync(ShipmentOrder order)
        {
            var shipmentOrderInfos = DbContext.Set<ShipmentOrderInfo>();
            await shipmentOrderInfos.Where(a => a.ShipmentOrderId == order.Id).DeleteAsync();
            shipmentOrderInfos.AddRange(order.ShipmentOrderInfos);
        }
    }
}
