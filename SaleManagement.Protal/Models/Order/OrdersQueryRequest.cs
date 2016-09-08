using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Order
{
    public class OrdersQueryRequest : PagingRequest
    {
        public string CustomerId { get; set; }

        public string OrderId { get; set; }

        public DateTime? DeliveryStartDate { get; set; }

        public DateTime? DeliveryEndDate { get; set; }

        public OrderStatus? Status { get; set; }

        public Func<IQueryable<Core.Models.Order>, IQueryable<Core.Models.Order>> GetOrderListQueryFilter(SaleUser user)
        {
            Func<IQueryable<Core.Models.Order>, IQueryable<Core.Models.Order>> filter = query =>
            {
                if (user.Role.Code == SaleManagentConstants.SystemRole.CustomerService)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.UnConfirmed ||
                             f.OrderStatus == OrderStatus.Distribution || f.OrderStatus == OrderStatus.Design || f.OrderStatus == OrderStatus.CustomerTobeConfirm);
                }

                if (user.Role.Code == SaleManagentConstants.SystemRole.SendAndReceive)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.OutputWax ||
                             f.OrderStatus == OrderStatus.WithTheHand || f.OrderStatus == OrderStatus.MicroInsert
                             || f.OrderStatus == OrderStatus.Polishing || f.OrderStatus == OrderStatus.Module || f.OrderStatus == OrderStatus.Pack);
                }

                if (user.Role.Code == SaleManagentConstants.SystemRole.Finance)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.ToBeShip);
                }

                if (!string.IsNullOrEmpty(CustomerId))
                {
                    query = query.Where(f => f.CustomerId == CustomerId);
                }

                if (!string.IsNullOrEmpty(OrderId))
                {
                    query = query.Where(f => f.Id.Contains(OrderId));
                }

                if (DeliveryStartDate.HasValue)
                {
                    query = query.Where(f => f.DeliveryDate >= DeliveryStartDate.Value);
                }

                if (DeliveryEndDate.HasValue)
                {
                    var endDate = DeliveryEndDate.Value.AddDays(1);
                    query = query.Where(f => f.DeliveryDate <= endDate);
                }

                if (Status.HasValue)
                {
                    query = query.Where(f => f.OrderStatus == Status);
                }
                return query.AsNoTracking();
            };
            return filter;
        }
    }
}