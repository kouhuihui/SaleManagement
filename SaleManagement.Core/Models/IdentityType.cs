using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public enum IdentityType
    {
        [Display(Name = "员工")]
        Employee = 1,

        [Display(Name = "客户")]
        Customer = 2
    }
}