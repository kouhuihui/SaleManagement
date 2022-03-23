using Dickson.Core.Common.Extensions;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using System.Linq;

namespace SaleManagement.Open.Models.SpotGood
{
    public class SpotGoodListItemViewModel
    {
        public SpotGoodListItemViewModel(SpotGoods spotGood)
        {
            Id = spotGood.Id;
            Name = spotGood.SpotGoodsPattern.Name;
            //SpotGoodsType = spotGood.SpotGoodsPattern.Type.GetDisplayName();
            HandSize = spotGood.HandSize;
            MainStone = spotGood.MainStone;
            Weight = spotGood.Weight;
            GoldWeight = spotGood.GoldWeight;
            Price = decimal.Round((decimal)spotGood.Price, 2);
            ColorFormName = spotGood.ColorForm.Name;
            ColorFormId = spotGood.ColorForm.Id;
            SetStoneNames = string.Join("/", spotGood.SetStoneInfos.Select(r => r.MatchStoneName));
            SetStoneNumbers = string.Join("/", spotGood.SetStoneInfos.Select(r => r.Number));
            SetStoneWeights = string.Join("/", spotGood.SetStoneInfos.Select(r => r.Weight));
            BasicCost = spotGood.BasicCost;
            Loss18KRate = spotGood.Loss18KRate;
        }


        public SpotGoodListItemViewModel(SpotGoodsOrder SpotGoodsOrder)
        {
            var spotGood = SpotGoodsOrder.SpotGoods;
            Id = spotGood.Id;
            Name = spotGood.SpotGoodsPattern.Name;
            //SpotGoodsType = spotGood.SpotGoodsPattern.Type.GetDisplayName();
            HandSize = spotGood.HandSize;
            MainStone = spotGood.MainStone;
            Weight = spotGood.Weight;
            GoldWeight = spotGood.GoldWeight;
            Price = SpotGoodsOrder.Price;
            ColorFormName = spotGood.ColorForm.Name;
            DailyGoldPrice = SpotGoodsOrder.GoldPrice;
            IsMosaic = SpotGoodsOrder.IsMosaic;
            ColorFormId = spotGood.ColorForm.Id;
            SetStoneNames = string.Join("/", spotGood.SetStoneInfos.Select(r => r.MatchStoneName));
            SetStoneNumbers = string.Join("/", spotGood.SetStoneInfos.Select(r => r.Number));
            SetStoneWeights = string.Join("/", spotGood.SetStoneInfos.Select(r => r.Weight));
            ImageUrl = SaleManagentConstants.Misc.SaleMangementWeb + "/Attachment/" + spotGood.SpotGoodsPattern.FileInfoId + "/preview";
            StatusName = spotGood.Status.GetDisplayName();
            BasicCost = spotGood.BasicCost;
            Loss18KRate = spotGood.Loss18KRate;
            SfNo = SpotGoodsOrder.SfNo;
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

        public double DailyGoldPrice { get; set; }

        public decimal Price { get; set; }

        public int ColorFormId { get; set; }

        public string ColorFormName { get; set; }

        public string SetStoneNames { get; set; }

        public string SetStoneNumbers { get; set; }

        public string SetStoneWeights { get; set; }

        public string ImageUrl { get; set; }

        public string StatusName { get; set; }

        public double BasicCost { get; set; }

        public int Loss18KRate { get; set; }

        public string SfNo { get; set; }
    }
}