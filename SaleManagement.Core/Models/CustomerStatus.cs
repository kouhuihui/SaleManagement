using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public enum CustomerStatus
    {
        [Display(Name = "已禁用")]
        Disabled = 0,

        [Display(Name = "已启用")]
        Enabled = 1,
    }
}
