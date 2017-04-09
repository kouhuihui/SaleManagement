using System.Collections.Generic;

namespace SaleManagement.Protal.Models.SpotGoods
{
    public class SpotGoodsViewModel : SpotGoodsListItemViewModel
    {
        public SpotGoodsViewModel()
        {
            SetStoneInfos = new List<SpotGoodsSetStoneInfoViewModel>();
        }

        public IEnumerable<SpotGoodsSetStoneInfoViewModel> SetStoneInfos { get; set; }
    }
}