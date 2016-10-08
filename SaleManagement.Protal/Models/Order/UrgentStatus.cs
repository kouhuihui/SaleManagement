using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Protal.Models.Order
{
    public enum UrgentStatus
    {
        [Display(Name = "正常")]
        Normal = 0,

        [Display(Name = "紧急")]
        Urgent = 1,

        [Display(Name = "非常紧急")]
        VeryUrgent = 2,
    }
}