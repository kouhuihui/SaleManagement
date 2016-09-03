using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public enum RoleType
    {
        [Display(Name = "管理")]
        Admin = 1,

        [Display(Name = "生产")]
        Production = 2
    }
}
