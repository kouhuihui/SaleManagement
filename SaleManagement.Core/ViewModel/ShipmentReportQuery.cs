using SaleManagement.Core.Models;
using System;
using System.Linq;

namespace SaleManagement.Core.ViewModel
{
    public class ShipmentReportQuery : ReportQueryBaseDto
    {
        public ShipmentReportQuery()
        {
            DateTime now = DateTime.Now;
            StatisticStartDate = new DateTime(now.Year, now.Month, 1);
            StatisticEndDate = StatisticStartDate.AddMonths(1).AddDays(-1);
        }

        public new DateTime StatisticStartDate { get; set; }

        public new DateTime StatisticEndDate { get; set; }

        public Func<IQueryable<Core.Models.ShipmentOrderInfo>, IQueryable<Core.Models.ShipmentOrderInfo>> GetShipmentOrderInfosQueryFilter()
        {
            Func<IQueryable<Core.Models.ShipmentOrderInfo>, IQueryable<Core.Models.ShipmentOrderInfo>> filter = query =>
            {
                query = query.Where(f => f.ShipmentOrder.AuditStatus == ShipmentOrderAduitStatus.Pass);

                if (!string.IsNullOrEmpty(CustomerId))
                {
                    query = query.Where(f => f.Order.CustomerId == CustomerId);
                }

                query = query.Where(f => f.ShipmentOrder.DeliveryDate >= StatisticStartDate);
                var endDate = StatisticEndDate.AddDays(1);
                query = query.Where(f => f.ShipmentOrder.DeliveryDate < endDate);

                return query;
            };
            return filter;
        }
    }
}
