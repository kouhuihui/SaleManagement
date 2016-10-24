using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Shipment
{
    public class ShipmentOrdersQueryRequest : PagingRequest
    {
        public string CustomerId { get; set; }

        public string ShipmentOrderId { get; set; }

        public string OrderId { get; set; }

        public DateTime? DeliveryStartDate { get; set; }

        public DateTime? DeliveryEndDate { get; set; }

        public ShipmentOrderAduitStatus? Status { get; set; }

        public Func<IQueryable<Core.Models.ShipmentOrder>, IQueryable<Core.Models.ShipmentOrder>> GetOrderListQueryFilter()
        {
            Func<IQueryable<Core.Models.ShipmentOrder>, IQueryable<Core.Models.ShipmentOrder>> filter = query =>
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    query = query.Where(f => f.CustomerId == CustomerId);
                }

                if (!string.IsNullOrEmpty(ShipmentOrderId))
                {
                    var shipmentOrderIds = ShipmentOrderId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    query = query.Where(f => shipmentOrderIds.Any(s => f.Id.Contains(s)));
                }

                if (!string.IsNullOrEmpty(OrderId))
                {
                    query = query.Where(f => f.ShipmentOrderInfos.Any(s => s.Id.Contains(OrderId)));
                }

                if (DeliveryStartDate.HasValue)
                {
                    query = query.Where(f => f.DeliveryDate >= DeliveryStartDate.Value);
                }

                if (DeliveryEndDate.HasValue)
                {
                    query = query.Where(f => f.DeliveryDate <= DeliveryEndDate.Value);
                }

                if (Status.HasValue)
                {
                    query = query.Where(f => f.AuditStatus == Status);
                }
                return query.AsNoTracking();
            };
            return filter;
        }
    }
}