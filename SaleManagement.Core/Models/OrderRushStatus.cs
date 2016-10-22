using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public enum OrderRushStatus
    {
        [Display(Name = "一般")]
        Normal = 0,

        [Display(Name = "紧急")]
        Rush = 1,

        [Display(Name = "特急")]
        VeryRush = 2
    }
}
