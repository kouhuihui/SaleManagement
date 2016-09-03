using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    /// <summary>
    /// 客来石描述
    /// </summary>
    public enum StoneDescribe
    {
        [Display(Name ="正常")]
        Normal = 0,

        [Display(Name = "多裂")]
        MoreCrack = 1,

        [Display(Name = "微裂")]
        TinyCrack = 2,

        [Display(Name = "无瑕")]
        NotCrack = 3
    }
}
