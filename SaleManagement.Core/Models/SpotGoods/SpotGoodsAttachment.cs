using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class SpotGoodsAttachment
    {
        public SpotGoodsAttachment()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }

        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string SpotGoodsId { get; set; }

        public virtual SpotGoods SpotGoods { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string FileInfoId { get; set; }

        public DateTime Created { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CreatorId { get; set; }
    }
}
