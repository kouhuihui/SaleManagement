using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public enum ReconciliationType
    {
        [Display(Name = "付款")]
        Payment = 0,

        [Display(Name = "欠款")]
        Arrearage = 1
    }
}
