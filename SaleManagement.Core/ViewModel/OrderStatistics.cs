using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.ViewModel
{
    public class OrderStatistics
    {
        public int UnConfirmedCount { get; set; }

        public int ProcessingCount { get; set; }

        public int ShipmentCount { get; set; }

        public int UrgentCount { get; set; }

        public int VeryUrgentCount { get; set; }
    }
}
