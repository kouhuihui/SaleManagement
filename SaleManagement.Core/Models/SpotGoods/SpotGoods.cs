using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    /// <summary>
    /// 现货
    /// </summary>
    public class SpotGoods
    {
        public SpotGoods()
        {
            Created = DateTime.Now;
        }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        public SpotGoodsType SpotGoodsType { get; set; }

        public int ColorFormId { get; set; }

        public virtual ColorForm ColorForm { get; set; }

        public int HandSize { get; set; }

        public string MainStone { get; set; }

        public double Weight { get; set; }

        public double GoldWeight { get; set; }

        public double MosaicCost { get; set; }

        public bool IsMosaic { get; set; }

        public string CreatorId { get; set; }

        public DateTime Created { get; set; }

        public bool IsLock { get; set; }

        public SpotGoodsStatus status { get; set; }

        public double Price { get; set; }

        public virtual ICollection<SpotGoodsAttachment> Attachments { get; set; }

        public virtual ICollection<SpotGoodsSetStoneInfo> SetStoneInfos { get; set; }
    }
}
