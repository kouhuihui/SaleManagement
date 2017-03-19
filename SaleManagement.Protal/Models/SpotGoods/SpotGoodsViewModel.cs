using SaleManagement.Core.ViewModel;
using System.Collections.Generic;

namespace SaleManagement.Protal.Models.SpotGoods
{
    public class SpotGoodsViewModel : SpotGoodsListItemViewModel
    {
        public SpotGoodsViewModel()
        {
            Attachments = new List<AttachmentItem>();
        }

        public IList<AttachmentItem> Attachments { get; set; }
    }
}