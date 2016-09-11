﻿using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections;
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

        public async Task<InvokedResult> UpdateOrderStatusAsync( OrderStatus status, IEnumerable<string> orderIds)
        {
            if (!orderIds.Any())
                return InvokedResult.SucceededResult;

            var dataSet = DbContext.Set<Order>();
            var orders = dataSet.Where(r => orderIds.Contains(r.Id));
            foreach(var order in orders)
            {
                order.OrderStatus = status;
                order.Updated = DateTime.Now;
            }

            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<Order> GetOrderAsync(string orderId)
        {
            return await DbContext.Set<Order>().FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string[] orderIds)
        {
            Requires.NotNull(orderIds, "orderIds");
            return await DbContext.Set<Order>().Where(o => orderIds.Contains( o.Id)).ToListAsync();
        }

        public async Task<Paging<Order>> GetOrdersAsync(int start, int take, Func<IQueryable<Order>, IQueryable<Order>> filter = null)
        {
            var query = DbContext.Set<Order>().Where(o => o.ComplayId == User.CompanyId);
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).Skip(start).Take(take).ToListAsync();

            return new Paging<Order>(start, take, total, list);
        }

        public async Task<bool> RemoveAttachment(OrderAttachment attachement)
        {
            DbContext.Set<OrderAttachment>().Remove(attachement);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public async Task<OrderStatistics> GetOrderStatisticsAsync()
        {
            var query = DbContext.Set<Order>().AsQueryable().Where(o=>o.ComplayId == User.CompanyId);
            var unConfirmedCountQuery = query.Where(o => o.OrderStatus == OrderStatus.UnConfirmed).Select(j => new { Key = "unconfirmed", Id = j.Id }); 
            var processingCountQuery = query.Where(o => o.OrderStatus != OrderStatus.UnConfirmed  && o.OrderStatus != OrderStatus.ToBeShip && o.OrderStatus != OrderStatus.Shipmenting && o.OrderStatus != OrderStatus.Shipment && o.OrderStatus != OrderStatus.HaveGoods).Select(j => new { Key = "processing", Id = j.Id });
            var shipmentCountQuery = query.Where(o => o.OrderStatus == OrderStatus.Shipment).Select(j => new { Key = "shipment", Id = j.Id });
            var unionList = await unConfirmedCountQuery.Union(processingCountQuery)
                .Union(shipmentCountQuery).GroupBy(a => a.Key).Select(g => new { Status = g.Key, Count = g.Count() }).ToListAsync();
            return new OrderStatistics()
            {
                UnConfirmedCount = unionList.FirstOrDefault(k => k.Status == "unconfirmed")?.Count ?? 0,
                ProcessingCount = unionList.FirstOrDefault(k => k.Status == "processing")?.Count ?? 0,
                ShipmentCount = unionList.FirstOrDefault(k => k.Status == "shipment")?.Count ?? 0,
            };
        }
    }
}
