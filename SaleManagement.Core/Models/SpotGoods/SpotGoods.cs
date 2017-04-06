using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public string SpotGoodsPatternId { get; set; }

        public virtual SpotGoodsPattern SpotGoodsPattern { get; set; }

        public int ColorFormId { get; set; }

        public virtual ColorForm ColorForm { get; set; }

        public int HandSize { get; set; }

        public string MainStone { get; set; }

        public double Weight { get; set; }

        public double GoldWeight { get; set; }

        /// <summary>
        /// 镶嵌费用
        /// </summary>
        public double MosaicCost { get; set; }

        /// <summary>
        /// 是否镶嵌
        /// </summary>
        public bool IsMosaic { get; set; }

        public string CreatorId { get; set; }

        public DateTime Created { get; set; }

        public bool IsLock { get; set; }

        public SpotGoodsStatus Status { get; set; }

        public double Price { get; set; }

        public virtual ICollection<SpotGoodsSetStoneInfo> SetStoneInfos { get; set; }
    }
}
