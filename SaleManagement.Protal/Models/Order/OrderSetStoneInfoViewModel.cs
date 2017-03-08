using SaleManagement.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Protal.Models.Order
{
    public class OrderSetStoneInfoViewModel
    {
        public int Id { get; set; }

        [Display(Name = "配石类型")]
        [Required(ErrorMessage = "请选择{0}")]
        public int MatchStoneId { get; set; }

        public string MatchStoneName { get; set; }

        [Display(Name = "配石数量")]
        [Required]
        [Range(1, 1000, ErrorMessage = "{0}范围是1-1000")]
        public int Number { get; set; }

        [Display(Name = "重量")]
        [Required]
        [Range(0.001, 1000, ErrorMessage = "{0}范围是0.001-1000")]
        public double Weight { get; set; }

        public string OrderId { get; set; }

        public double Price { get; set; }

        public double TotalAmount { get; set; }

        public double SetStoneWorkingCost { get; set; }

        public IEnumerable<MatchStone> MatchStones { get; set; }
    }
}