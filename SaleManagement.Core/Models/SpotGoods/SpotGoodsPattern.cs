
using System.Collections.Generic;
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

        public int ProductCategoryId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public int GemCategoryId { get; set; }

        public virtual GemCategory GemCategory { get; set; }

        public double Price { get; set; }

        public string ReferenceData { get; set; }

        public int RowNo { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string FileInfoId { get; set; }

        public virtual ICollection<SpotGoodsPatternType> SpotGoodsPatternTypes { get; set; }
    }
}
