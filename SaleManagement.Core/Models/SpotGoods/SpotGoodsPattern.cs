using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class SpotGoodsPattern
    {
        public string Id { get; set; }

        [Display(Name = "款式名称")]
        [Required(ErrorMessage = "{0}是必须的")]
        [StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        [Display(Name = "现货类别")]
        public SpotGoodsType Type { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string FileInfoId { get; set; }
    }
}
