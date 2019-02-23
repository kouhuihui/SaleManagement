using SaleManagement.Core;
using SaleManagement.Core.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace SaleManagement.Protal.Models.Order
{
    public class OrdersQueryRequest : OrderQueryRequestBase
    {
        public DateTime? DeliveryStartDate { get; set; }

        public DateTime? DeliveryEndDate { get; set; }

        public DateTime? OutPutWaxDate { get; set; }

        public UrgentStatus? UrgentStatus { get; set; }

        public OrderRushStatus? RushStatus { get; set; }

        public string CurrentUserId { get; set; }

        public bool IsProcess { get; set; }

        public Func<IQueryable<Core.Models.Order>, IQueryable<Core.Models.Order>> GetOrderListQueryFilter(SaleUser user)
        {
            Func<IQueryable<Core.Models.Order>, IQueryable<Core.Models.Order>> filter = query =>
            {
                if (user.Role.Code == SaleManagentConstants.SystemRole.CustomerService)
                {
                    query = query.Where(f => f.OrderStatus != OrderStatus.Delete && f.OrderStatus != OrderStatus.ToBeShip && f.OrderStatus != OrderStatus.Shipmenting && f.OrderStatus != OrderStatus.Shipment && f.OrderStatus != OrderStatus.HaveGoods);
                }
                if (user.Role.Code == SaleManagentConstants.SystemRole.Design)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.Design || f.OrderStatus == OrderStatus.DirectorTobeConfirm || f.OrderStatus == OrderStatus.CustomerTobeConfirm || f.OrderStatus == OrderStatus.CustomerConfirm);
                }
                if (user.Role.Code == SaleManagentConstants.SystemRole.SendAndReceive)
                {
                    query = query.Where(f => f.OrderStatus == OrderStatus.OutputWax ||
                             f.OrderStatus == OrderStatus.WithTheHand || f.OrderStatus == OrderStatus.MicroInsert
                             || f.OrderStatus == OrderStatus.Polishing || f.OrderStatus == OrderStatus.Module
                             || f.OrderStatus == OrderStatus.Pack || f.OrderStatus == OrderStatus.DumpModule
                             || f.OrderStatus == OrderStatus.WaitStone);
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
                    var orderIds = OrderId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    query = query.Where(f => orderIds.Any(o => f.Id.Contains(o)));
                }

                if (DeliveryStartDate.HasValue)
                {
                    query = query.Where(f => f.DeliveryDate >= DeliveryStartDate.Value);
                }

                if (DeliveryEndDate.HasValue)
                {
                    var endDate = DeliveryEndDate.Value.AddDays(1);
                    query = query.Where(f => f.DeliveryDate < endDate);
                }

                if (ColorFormId.HasValue)
                {
                    query = query.Where(f => f.ColorFormId == ColorFormId.Value);
                }

                if (IsProcess)
                {
                    query = query.Where(o => o.OrderStatus != OrderStatus.UnConfirmed && o.OrderStatus != OrderStatus.Shipment && o.OrderStatus != OrderStatus.HaveGoods && o.OrderStatus != OrderStatus.Delete);
                }

                if (Status.HasValue)
                {
                    query = query.Where(f => f.OrderStatus == Status);
                }
                else
                {
                    query = query.Where(f => f.OrderStatus != OrderStatus.Delete);
                }

                if (UrgentStatus.HasValue)
                {
                    query = GetUrgentOrderQuery(query, UrgentStatus.Value);
                }

                if (RushStatus.HasValue)
                {
                    query = query.Where(f => f.OrderStatus != OrderStatus.Delete && f.OrderStatus != OrderStatus.HaveGoods && f.OrderStatus != OrderStatus.Shipment && f.OrderRushStatus == RushStatus.Value);
                }

                if (!string.IsNullOrEmpty(CurrentUserId))
                {
                    query = query.Where(f => f.CurrentUserId == CurrentUserId);
                }

                return query.AsNoTracking();
            };
            return filter;
        }

        private IQueryable<Core.Models.Order> GetUrgentOrderQuery(
           IQueryable<Core.Models.Order> query, UrgentStatus ugentStatus)
        {
            query = query.Where(f => f.OrderStatus != OrderStatus.Delete && f.OrderStatus != OrderStatus.HaveGoods && f.OrderStatus != OrderStatus.Shipment);
            var now = DateTime.Now.Date;
            DateTime ugentWarningStartDate;
            DateTime ugentWarningEndDate;
            switch (ugentStatus)
            {
                case Order.UrgentStatus.Normal:
                    ugentWarningStartDate = now.AddDays(SaleManagentConstants.UI.OrderUrgentWaringDay);
                    query = query.Where(f => f.DeliveryDate > ugentWarningStartDate);
                    break;
                case Order.UrgentStatus.Urgent:
                    ugentWarningEndDate = now.AddDays(SaleManagentConstants.UI.OrderUrgentWaringDay);
                    ugentWarningStartDate = now.AddDays(SaleManagentConstants.UI.OrderVeryUrgentWaringDay);
                    query = query.Where(f => f.DeliveryDate > ugentWarningStartDate && f.DeliveryDate <= ugentWarningEndDate);
                    break;
                case Order.UrgentStatus.VeryUrgent:
                    ugentWarningStartDate = now.AddDays(SaleManagentConstants.UI.OrderVeryUrgentWaringDay);
                    query = query.Where(f => f.DeliveryDate <= ugentWarningStartDate);
                    break;
            }
            return query;
        }
    }
}