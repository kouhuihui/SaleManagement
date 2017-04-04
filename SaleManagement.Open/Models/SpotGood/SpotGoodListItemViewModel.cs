using Dickson.Core.Common.Extensions;
using SaleManagement.Core.Models;

namespace SaleManagement.Open.Models.SpotGood
{
    public class SpotGoodListItemViewModel
    {
        public SpotGoodListItemViewModel(SpotGoods spotGood)
        {
            Id = spotGood.Id;
            Name = spotGood.SpotGoodsPattern.Name;
            SpotGoodsType = spotGood.SpotGoodsPattern.Type.GetDisplayName();
            HandSize = spotGood.HandSize;
            MainStone = spotGood.MainStone;
            Weight = spotGood.Weight;
            GoldWeight = spotGood.GoldWeight;
            IsMosaic = spotGood.IsMosaic;
            Price = spotGood.Price;
            ColorFormName = spotGood.ColorForm.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string SpotGoodsType { get; set; }

        public int HandSize { get; set; }

        public string MainStone { get; set; }

        public double Weight { get; set; }

        public double GoldWeight { get; set; }

        /// <summary>
        /// 镶嵌费用
        /// </summary>
        public double MosaicCost { get; set; }

        public bool IsMosaic { get; set; }

        public double Price { get; set; }

        public string ColorFormName { get; set; }
    }
}