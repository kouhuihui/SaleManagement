using SaleManagement.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Protal.Models
{

    public class OrderMainStoneInfoViewModel
    {
        public int Id { get; set; }

        public string MainStoneName { get; set; }


        [Display(Name = "主石类型")]
        [Required(ErrorMessage = "请选择{0}")]
        public int MainStoneId { get; set; }

        [Display(Name = "重量")]
        [Required]
        [Range(0.001, 1000, ErrorMessage = "{0}范围是0.001-1000")]
        public double Weight { get; set; }

        public RiskType RiskType { get; set; }

        public string Created { get; set; }

        public string OrderId { get; set; }

        public IEnumerable<MainStone> MainStones { get; set; }
    }
}