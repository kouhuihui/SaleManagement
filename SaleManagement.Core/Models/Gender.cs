using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public enum Gender
    {
        [Display(Name ="男性")]
        Male = 1,

        [Display(Name = "女性")]
        Female = 2
    }
}
