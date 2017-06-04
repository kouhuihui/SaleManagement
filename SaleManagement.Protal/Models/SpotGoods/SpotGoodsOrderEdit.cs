using SaleManagement.Core.Models;

namespace SaleManagement.Protal.Models.SpotGoods
{
    public class SpotGoodsOrderEdit
    {
        public string SpotGoodsId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string Address { get; set; }

        public SpotGoodsStatus Status { get; set; }

        public string SfNo { get; set; }

    }
}