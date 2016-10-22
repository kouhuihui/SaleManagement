namespace SaleManagement.Core.ViewModel
{
    public class OrderStatistics
    {
        public int UnConfirmedCount { get; set; }

        public int ProcessingCount { get; set; }

        public int ShipmentCount { get; set; }

        public int UrgentCount { get; set; }

        public int VeryUrgentCount { get; set; }

        public int RushCount { get; set; }

        public int VeryRushCount { get; set; }
    }
}
