using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class SpotGoodsOrder
    {
        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        public string SpotGoodsId { get; set; }

        public SpotGoods SpotGoods { get; set; }


        [Required, StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string ProductNo { get; set; }

        /// <summary>
        /// 是否镶嵌
        /// </summary>
        public bool IsMosaic { get; set; }

        public double GoldPrice { get; set; }

        public decimal Price { get; set; }

        public DateTime Created { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string OpenId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string Address { get; set; }
    }
}
