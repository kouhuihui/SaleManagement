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
    public class OrderOperationLogManager : BaseManager
    {
        public OrderOperationLogManager(SaleUser user) : base(user)
        {
        }

        public async Task<InvokedResult> AddLogAsync(OrderStatus status,string orderId)
        {
            string content = GetContent(status);
            var log = Create(status, orderId, content);
            DbContext.Set<OrderOperationLog>().Add(log);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> AddLogAsync(OrderStatus status, IEnumerable<string> orderIds)
        {
            if (!orderIds.Any())
                return InvokedResult.SucceededResult;

            string content = GetContent(status);
            foreach (var orderId in orderIds)
            {
                var log = Create(status, orderId, content);
                DbContext.Set<OrderOperationLog>().Add(log);
            }
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<IEnumerable<OrderOperationLog>> GetOrderOperationLogs(string orderId)
        {
            Requires.NotNullOrEmpty(orderId, "orderId");

            return await DbContext.Set<OrderOperationLog>().Where(o => o.OrderId == orderId).OrderByDescending(r => r.Created).ToListAsync();
        }

        private string GetContent(OrderStatus status)
        {
            string content = "";
            switch (status)
            {
                case OrderStatus.UnConfirmed:
                    content = "创建订单";
                    break;
                case OrderStatus.Design:
                    content = "进入设计部门";
                    break;
                case OrderStatus.CustomerTobeConfirm:
                    content = "设计师完成设计";
                    break;
                case OrderStatus.OutputWax:
                    content = "进入出蜡部门";
                    break;
                case OrderStatus.Module:
                    content = "进入执模部门";
                    break;
                case OrderStatus.WithTheHand:
                    content = "进入手镶部门";
                    break;
                case OrderStatus.MicroInsert:
                    content = "进入微镶部门";
                    break;
                case OrderStatus.Polishing:
                    content = "进入抛光部门";
                    break;
                case OrderStatus.Pack:
                    content = "进入打包部门";
                    break;
                case OrderStatus.ToBeShip:
                    content = "进入待出货阶段";
                    break;
                case OrderStatus.Shipmenting:
                    content = "出货单生成阶段";
                    break;
                case OrderStatus.Shipment:
                    content = "已出货";
                    break;
                case OrderStatus.HaveGoods:
                    content = "已收货";
                    break;
            }
            return content;
        }

        private OrderOperationLog Create(OrderStatus status, string orderId,string content)
        {
            return new OrderOperationLog
            {
                Content = content,
                Created = DateTime.Now,
                CreatorId = User.Id,
                CreatorName = User.Name,
                Status = status,
                OrderId = orderId
            };
        }
    }
}
