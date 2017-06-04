using System;

namespace SaleManagement.Open.Models.SpotGood
{
    public class SpotGoodsOrderViewModel
    {
        public SpotGoodsOrderViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string SpotGoodsId { get; set; }

        public string ProductNo { get; set; }

        public bool IsMosaic { get; set; }

        public double GoldPrice { get; set; }

        public decimal Price { get; set; }

        public DateTime Created { get; set; }

        public string OpenId { get; set; }

        public string CustomerName { get; set; }
    }
}