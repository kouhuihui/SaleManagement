using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    /// <summary>
    /// 戒臂内弧要求
    /// </summary>
    public enum RadianRequirement
    {
        [Display(Name = "一般")]
        Normal = 0,

        [Display(Name = "无弧")]
        None = 1,

        [Display(Name = "加大")]
        Increase = 2
    }
}
