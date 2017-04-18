namespace SaleManagement.Protal.Models
{
    public class SpotGoodsOrderViewModel
    {
        public string Id { get; set; }

        public string SpotGoodsId { get; set; }

        public string ProductNo { get; set; }

        /// <summary>
        /// 是否镶嵌
        /// </summary>
        public bool IsMosaic { get; set; }

        public decimal Price { get; set; }

        public string StatusName { get; set; }

        public string Created { get; set; }

        public string OpenId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string Address { get; set; }
    }
}