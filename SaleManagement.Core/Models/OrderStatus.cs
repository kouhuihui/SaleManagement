using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public enum OrderStatus
    {
        [Display(Name = "未确认生产")]
        UnConfirmed = 0,

        [Display(Name = "分单")]
        Distribution = 1,

        [Display(Name = "设计师设计")]
        Design = 2,

        [Display(Name = "客户待确认")]
        CustomerTobeConfirm = 3,

        [Display(Name = "客户已确认")]
        CustomerConfirm = 4,

        [Display(Name = "出蜡")]
        OutputWax = 5,

        [Display(Name = "执模")]
        Module = 6,

        [Display(Name = "镶石")]
        SetStone = 7,

        [Display(Name = "打包")]
        Pack = 8,

        [Display(Name = "待出货")]
        ToBeShip = 9,

        [Display(Name = "出货中")]
        Shipmenting = 10,

        [Display(Name = "已出货")]
        Shipment = 11,

        [Display(Name = "已收货")]
        HaveGoods = 12,
    }
}
