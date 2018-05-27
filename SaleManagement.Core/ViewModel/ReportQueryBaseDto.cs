using System;

namespace SaleManagement.Core.ViewModel
{
    public class ReportQueryBaseDto
    {
        public ReportQueryBaseDto()
        {
            DateTime now = DateTime.Now;
            StatisticStartDate = new DateTime(now.Year, now.Month, 1);
            StatisticEndDate = StatisticStartDate.Value.AddMonths(1).AddDays(-1);
        }

        public string CustomerId { get; set; }

        public DateTime? StatisticStartDate { get; set; }

        public DateTime? StatisticEndDate { get; set; }

        public string OrderId { get; set; }
    }
}
