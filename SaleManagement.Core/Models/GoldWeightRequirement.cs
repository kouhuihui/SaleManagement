using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    /// <summary>
    /// 金石控制要求
    /// </summary>
    public enum GoldWeightRequirement
    {
        [Display(Name = "正常")]
        Normal =0,

        [Display(Name = "轻金")]
        LightGold =1,

        [Display(Name ="加厚")]
        Thicken = 2,
    }
}
