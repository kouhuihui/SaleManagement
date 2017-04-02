using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public enum FilePurpose
    {
        [Display(Name = "Order.Attachment")]
        OrderAttachment = 1,

        [Display(Name = "SpotGoods.Attachment")]
        SpotGoodsAttachment = 2
    }
}
