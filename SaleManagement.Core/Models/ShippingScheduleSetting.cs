using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class ShippingScheduleSetting
    {
        public ShippingScheduleSetting()
        {
            Days = 20;
            Created = DateTime.Now;
        }

        public int Id { get; set; }

        [Display(Name = "出货工期")]
        [Required(ErrorMessage = "请输入{0}")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "请输入整数")]
        [Range(5, 100, ErrorMessage = "{0}范围是5-100")]
        public int Days { get; set; }

        public DateTime Created { get; set; }

        public string UserId { get; set; }

        public int CompanyId { get; set; }
    }
}
