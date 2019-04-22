using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public enum FilePurpose
    {
        [Display(Name = "Order.Attachment")]
        OrderAttachment = 1,

        [Display(Name = "SpotGoods.Attachment")]
        SpotGoodsAttachment = 2,

        [Display(Name = "Order.MainStonAttachment")]
        OrderMainStoneAttachment = 3,

        [Display(Name = "HotSelling.Attachment")]
        HotSellingAttachment = 4,

        [Display(Name = "HotSelling.ParamAttachment")]
        HotSellingParamAttachment = 5,
    }
}
