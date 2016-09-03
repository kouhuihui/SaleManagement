using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    /// <summary>
    /// 副石控制要求
    /// </summary>
    public enum SideStoneRequiredment
    {
        [Display(Name = "正常")]
        Normal =0,

        [Display(Name = "少石")]
        Little =1,

        [Display(Name = "加重")]
        Increase = 2,
    }
}
