using Dickson.Core.Common.Extensions;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace SaleManagement.Open.Models.SpotGood
{
    public class SpotGoodListItemViewModel
    {
        public SpotGoodListItemViewModel(SpotGoods spotGood)
        {
            Id = spotGood.Id;
            Name = spotGood.Name;
            SpotGoodsType = spotGood.SpotGoodsType.GetDisplayName();
            HandSize = spotGood.HandSize;
            MainStone = spotGood.MainStone;
            Weight = spotGood.Weight;
            GoldWeight = spotGood.GoldWeight;
            IsMosaic = spotGood.IsMosaic;
            Price = spotGood.Price;
            ColorFormName = spotGood.ColorForm.Name;
            Attachments = spotGood.SpotGoodsAttachments.Select(r => new AttachmentItem
            {
                Id = r.FileInfoId,
                Url = SaleManagentConstants.Misc.SaleMangementWeb + "/Attachment/" + r.FileInfoId + "/preview"
            });
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

        public IEnumerable<AttachmentItem> Attachments { get; set; }
    }
}