using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.ViewModel
{
    public class SetStoneReportQuery : ReportQueryBaseDto
    {
        public SetStoneReportQuery()
        {
            DateTime now = DateTime.Now;
            StatisticStartDate = new DateTime(now.Year, now.Month, 1);
            StatisticEndDate = StatisticStartDate.Value.AddMonths(1).AddDays(-1);
        }
    }
}
