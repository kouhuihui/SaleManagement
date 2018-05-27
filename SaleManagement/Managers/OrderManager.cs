using Dickson.Core.ComponentModel;
using EntityFramework.Extensions;
using SaleManagement.Core;
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
    public class OrderManager : BaseManager
    {
        public OrderManager(SaleUser user) : base(user)
        {

        }

        public async Task<InvokedResult> CreateOrder(Order order)
        {
            order.CreatorId = User.Id;
            order.CreatorName = User.Name;
            order.ComplayId = User.CompanyId;
            DbContext.Set<Order>().AddOrUpdate(order);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> UpdateOrderAsync(Order order)
        {
            DbContext.Set<Order>().AddOrUpdate(order);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> UpdateOrdersAsync(IEnumerable<Order> orders)
        {
            Requires.NotNull(orders, "orders");
            foreach (var order in orders)
            {
                DbContext.Set<Order>().AddOrUpdate(order);
            }
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> UpdateOrderStatusAsync(OrderStatus status, IEnumerable<string> orderIds)
        {
            if (!orderIds.Any())
                return InvokedResult.SucceededResult;

            var dataSet = DbContext.Set<Order>();
            var orders = dataSet.Where(r => orderIds.Contains(r.Id));
            foreach (var order in orders)
            {
                order.OrderStatus = status;
                order.Updated = DateTime.Now;
            }

            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<Order> GetOrderAsync(string orderId)
        {
            return await DbContext.Set<Order>().FirstOrDefaultAsync(o => o.ComplayId == User.CompanyId && o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string[] orderIds)
        {
            Requires.NotNull(orderIds, "orderIds");
            return await DbContext.Set<Order>().Where(o => o.ComplayId == User.CompanyId && orderIds.Contains(o.Id)).ToListAsync();
        }

        public async Task<Paging<Order>> GetDesignOrdersAsync(int start, int take, Func<IQueryable<Order>, IQueryable<Order>> filter = null)
        {
            var query = DbContext.Set<Order>().Where(o => o.ComplayId == User.CompanyId
           && (o.OrderStatus == OrderStatus.Design || o.OrderStatus == OrderStatus.CustomerTobeConfirm || o.OrderStatus == OrderStatus.CustomerConfirm));
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).Skip(start).Take(take).ToListAsync();

            return new Paging<Order>(start, take, total, list);
        }

        public async Task<Paging<Order>> GetOrdersAsync(int start, int take, Func<IQueryable<Order>, IQueryable<Order>> filter = null, DateTime? outPutWaxDate = null)
        {
            var query = DbContext.Set<Order>().Where(o => o.ComplayId == User.CompanyId);
            if (filter != null)
            {
                query = filter(query);
            }
            if (outPutWaxDate.HasValue)
            {
                var outPutWaxEndDate = outPutWaxDate.Value.AddDays(1);
                var outPutWaxOrderIds = DbContext.Set<OrderOperationLog>().Where(o => o.Status == OperationLogStatus.OutputWax && o.Created >= outPutWaxDate
                & o.Created < outPutWaxEndDate).Select(o => o.OrderId);
                query = query.Where(o => outPutWaxOrderIds.Any(r => r == o.Id));
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Id).Skip(start).Take(take).ToListAsync();

            return new Paging<Order>(start, take, total, list);
        }


        public async Task<bool> RemoveAttachment(OrderAttachment attachement)
        {
            DbContext.Set<OrderAttachment>().Remove(attachement);
            return await DbContext.SaveChangesAsync() > 0;
        }


        public async Task<OrderStatistics> GetOrderStatisticsAsync()
        {

            var query = DbContext.Set<Order>().AsQueryable().Where(o => o.ComplayId == User.CompanyId);
            var unConfirmedCountQuery = query.Where(o => o.OrderStatus == OrderStatus.UnConfirmed).Select(j => new { Key = "unconfirmed", Number = j.Number });
            var processingCountQuery = query.Where(o => o.OrderStatus != OrderStatus.UnConfirmed && o.OrderStatus != OrderStatus.Shipment && o.OrderStatus != OrderStatus.HaveGoods && o.OrderStatus != OrderStatus.Delete).Select(j => new { Key = "processing", Number = j.Number });
            var shipmentCountQuery = query.Where(o => o.OrderStatus == OrderStatus.ToBeShip).Select(j => new { Key = "shipment", Number = j.Number });

            var now = DateTime.Now.Date; ;
            var urgentWarningEndDate = now.AddDays(SaleManagentConstants.UI.OrderUrgentWaringDay);
            var urgentWarningStartDate = now.AddDays(SaleManagentConstants.UI.OrderVeryUrgentWaringDay);
            var urgentCountQuery = query.Where(f => f.OrderStatus != OrderStatus.Delete && f.OrderStatus != OrderStatus.HaveGoods && f.OrderStatus != OrderStatus.Shipment && f.DeliveryDate > urgentWarningStartDate && f.DeliveryDate <= urgentWarningEndDate).Select(j => new { Key = "urgent", Number = j.Number });

            var veryUrgentCountQuery = query.Where(f => f.OrderStatus != OrderStatus.Delete && f.OrderStatus != OrderStatus.HaveGoods && f.OrderStatus != OrderStatus.Shipment && f.DeliveryDate <= urgentWarningStartDate).Select(j => new { Key = "veryUrgent", Number = j.Number });

            var rushCountQuery = query.Where(f => f.OrderStatus != OrderStatus.Delete && f.OrderStatus != OrderStatus.HaveGoods && f.OrderStatus != OrderStatus.Shipment && f.OrderRushStatus == OrderRushStatus.Rush).Select(j => new { Key = "rush", Number = j.Number });

            var veryRushCountQuery = query.Where(f => f.OrderStatus != OrderStatus.Delete && f.OrderStatus != OrderStatus.HaveGoods && f.OrderStatus != OrderStatus.Shipment && f.OrderRushStatus == OrderRushStatus.VeryRush).Select(j => new { Key = "veryRush", Number = j.Number });

            var unionList = await unConfirmedCountQuery.Concat(processingCountQuery)
                .Concat(shipmentCountQuery).Concat(urgentCountQuery).Concat(veryUrgentCountQuery)
                .Concat(rushCountQuery).Concat(veryRushCountQuery)
                .GroupBy(a => a.Key).Select(g => new { Status = g.Key, Count = g.Sum(a => a.Number) }).ToListAsync();
            return new OrderStatistics()
            {
                UnConfirmedCount = unionList.FirstOrDefault(k => k.Status == "unconfirmed")?.Count ?? 0,
                ProcessingCount = unionList.FirstOrDefault(k => k.Status == "processing")?.Count ?? 0,
                ShipmentCount = unionList.FirstOrDefault(k => k.Status == "shipment")?.Count ?? 0,
                UrgentCount = unionList.FirstOrDefault(k => k.Status == "urgent")?.Count ?? 0,
                VeryUrgentCount = unionList.FirstOrDefault(k => k.Status == "veryUrgent")?.Count ?? 0,
                RushCount = unionList.FirstOrDefault(k => k.Status == "rush")?.Count ?? 0,
                VeryRushCount = unionList.FirstOrDefault(k => k.Status == "veryRush")?.Count ?? 0,
            };
        }

        public async Task<IEnumerable<OrderCalendar>> GetOrderCalendarsAsync(DateTime start, DateTime end)
        {
            var query = DbContext.Set<Order>().AsQueryable().Where(o => o.ComplayId == User.CompanyId && o.DeliveryDate >= start && o.DeliveryDate <= end);

            var filter = GetNoShipOrderFilter();
            if (filter != null)
            {
                query = filter(query);
            }

            var deliveryDates = await query.Select(r => new { DeliveryDate = r.DeliveryDate.Value, Number = r.Number }).ToListAsync();
            return deliveryDates.GroupBy(o => o.DeliveryDate).Select(w => new OrderCalendar
            {
                Date = w.Key,
                ProcessCount = w.Sum(a => a.Number)
            });
        }

        public async Task<InvokedResult> SetDeliverDay(string[] orderIds, int day)
        {
            var deliverDate = DateTime.Now.AddDays(day).Date;
            await DbContext.Set<Order>().Where(o => o.ComplayId == User.CompanyId && orderIds.Contains(o.Id)).UpdateAsync(u => new Order { DeliveryDate = deliverDate });
            return InvokedResult.SucceededResult;
        }

        private Func<IQueryable<Order>, IQueryable<Order>> GetNoShipOrderFilter()
        {
            Func<IQueryable<Core.Models.Order>, IQueryable<Core.Models.Order>> filter = query => query.Where(o => o.OrderStatus != OrderStatus.Shipment && o.OrderStatus != OrderStatus.HaveGoods && o.OrderStatus != OrderStatus.Delete);
            return filter;
        }
    }
}
