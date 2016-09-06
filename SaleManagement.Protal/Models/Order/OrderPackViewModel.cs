using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Order
{
    public class OrderPackViewModel
    {
        public string OrderId { get; set; }

        [Display(Name = "总重(g)")]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数的数字")]
        [Required(ErrorMessage = "请输入{0}")]
        public double Weight { get; set; }

        [Display(Name = "净金重(g)")]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数的数字")]
        [Required(ErrorMessage = "请输入{0}")]
        public double GoldWeight { get; set; }
    }
}