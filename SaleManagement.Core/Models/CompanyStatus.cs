using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public enum CompanyStatus
    {
        [Display(Name = "已禁用")]
        Disabled = 0,

        [Display(Name = "正常")]
        Normal = 1,
    }
}
