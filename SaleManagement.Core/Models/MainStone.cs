using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class MainStone
    {
        public int Id { get; set; }

        [Display(Name = "主石名称")]
        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(SaleManagentConstants.Validations.DefaultNameStringLength, ErrorMessage = "{0}最多支持{1}字符")]

        public string Name { get; set; }

        public RiskType RiskType { get; set; }

        public bool Deleted { get; set; }
    }
}
