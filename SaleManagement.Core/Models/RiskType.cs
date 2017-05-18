using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public enum RiskType
    {
        [Display(Name = "低风险")]
        Low = 0,

        [Display(Name = "中风险")]
        Middle = 1,

        [Display(Name = "高风险")]
        High = 2
    }
}
