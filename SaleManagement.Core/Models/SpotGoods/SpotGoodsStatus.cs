using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public enum SpotGoodsStatus
    {
        [Display(Name = "新建")]
        New = 0,

        [Display(Name = "已卖出")]
        Sell = 1,

        [Display(Name = "待自取")]
        PickBySelf = 2,

        [Display(Name = "待发货")]
        SF = 3,

        [Display(Name = "已自取")]
        HasTaken = 4,

        [Display(Name = "已顺丰")]
        HasSendGoods = 5
    }
}
