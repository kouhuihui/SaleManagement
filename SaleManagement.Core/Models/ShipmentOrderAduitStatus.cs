using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public enum ShipmentOrderAduitStatus
    {
        [Display(Name = "未审核")]
        New = 0,

        [Display(Name = "已审核")]
        Pass = 1,
    }
}
