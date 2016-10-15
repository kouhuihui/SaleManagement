using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Order
{
    public class StopOrderViewModel
    {
        public StopOrderViewModel()
        {
            DesginAmount = 0;
            OtherAmount = 0;
        }

        [Display(Name = "订单号")]
        [Required(ErrorMessage ="{0}不能为空")]
        public string OrderId { get; set; }

        [Display(Name = "设计费用")]
        [Required]
        [Range(0, 1000, ErrorMessage = "{0}范围是0-1000")]
        public double DesginAmount { get; set; }

        [Display(Name = "其他费用")]
        [Required]
        [Range(0, 10000, ErrorMessage = "{0}范围是0-10000")]
        public double OtherAmount { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }
    }
}