using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class SpotGoodsPatternType
    {
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string SpotGoodsPatternId { get; set; }

        public virtual SpotGoodsPattern SpotGoodsPattern { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string SpotGoodsTypeId { get; set; }

        public virtual SpotGoodsType SpotGoodsType { get; set; }
    }
}
