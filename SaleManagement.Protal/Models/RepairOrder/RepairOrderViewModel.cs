using SaleManagement.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.RepairOrder
{
    public class RepairOrderViewModel
    {
        public RepairOrderViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Display(Name = "品类")]
        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string ProductName { get; set; }

        [Display(Name = "金重(g)")]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数的数字")]
        [Required(ErrorMessage = "请输入{0}")]
        public double GoldWeight { get; set; }

        [Display(Name = "金额")]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数的数字")]
        [Required(ErrorMessage = "请输入{0}")]
        public double GoldAmount { get; set; }

        public int StoneNumber { get; set; }

        public double StoneWeight { get; set; }

        [Display(Name = "石值")]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数的数字")]
        [Required(ErrorMessage = "请输入{0}")]
        public double StoneAmount { get; set; }

        public string Remark { get; set; }

        [Display(Name = "总额")]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数的数字")]
        [Required(ErrorMessage = "请输入{0}")]
        public double TotalAmount { get; set; }

        public string ShipmentOrderId { get; set; }
    }
}