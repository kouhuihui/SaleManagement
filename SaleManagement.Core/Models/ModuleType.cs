using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public enum ModuleType
    {
        [Display(Name="唧蜡版")]
        Jina = 0 ,

        [Display(Name = "出蜡版")]
        Exist = 1,

        [Display(Name = "改蜡版")]
        Change =2,

        [Display(Name = "新做版")]
        New =3,

        [Display(Name = "雕蜡版")]
        Carving =4,

        [Display(Name = "自来蜡")]
        Customer =5
    }
}
