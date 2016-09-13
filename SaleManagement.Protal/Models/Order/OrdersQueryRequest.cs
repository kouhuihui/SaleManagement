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
    public class OrdersQueryRequest : OrderQueryRequestBase
    {
        public DateTime? DeliveryStartDate { get; set; }

        public DateTime? DeliveryEndDate { get; set; }

        public  Func<IQueryable<Core.Models.Order>, IQueryable<Core.Models.Order>> GetOrderListQueryFilter(SaleUser user)
        {
            Func<IQueryable<Core.Models.Order>, IQueryable<Core.Models.Order>> filter = query =>
            {
                if (user.Role.Code == SaleManagentConstants.SystemRole.CustomerService)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.UnConfirmed || f.OrderStatus == OrderStatus.Design || f.OrderStatus == OrderStatus.CustomerTobeConfirm);
                }
                if (user.Role.Code == SaleManagentConstants.SystemRole.Design)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.Design ||f.OrderStatus == OrderStatus.CustomerTobeConfirm || f.OrderStatus == OrderStatus.CustomerConfirm);
                }
                if (user.Role.Code == SaleManagentConstants.SystemRole.SendAndReceive)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.OutputWax ||
                             f.OrderStatus == OrderStatus.WithTheHand || f.OrderStatus == OrderStatus.MicroInsert
                             || f.OrderStatus == OrderStatus.Polishing || f.OrderStatus == OrderStatus.Module 
                             || f.OrderStatus == OrderStatus.Pack|| f.OrderStatus == OrderStatus.DumpModule);
                }

                if (user.Role.Code == SaleManagentConstants.SystemRole.Finance)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.ToBeShip);
                }

                if (user.Role.Code == SaleManagentConstants.SystemRole.OutputWax)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.OutputWax);
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

                if (!string.IsNullOrEmpty(ColorFormId))
                {
                    query = query.Where(f => f.CustomerId == ColorFormId);
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