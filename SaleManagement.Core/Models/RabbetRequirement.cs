using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    /// <summary>
    /// 主石镶口要求
    /// </summary>
    public enum RabbetRequirement
    {
        [Display(Name = "正常")]
        Normal = 0,

        [Display(Name = "低沉")]
        SinkLow = 1,

        [Display(Name = "抬高")]
        Raise = 2
    }
}
