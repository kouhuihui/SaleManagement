using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public enum OperationLogStatus
    {
        [Display(Name = "订单已删除")]
        Delete = -1,

        [Display(Name = "创建订单")]
        UnConfirmed = 0,

        [Display(Name = "进入设计部门")]
        Design = 2,

        [Display(Name = "设计图纸主管审核通过")]
        CustomerTobeConfirm = 3,

        [Display(Name = "客户确认设计")]
        CustomerConfirm = 4,

        [Display(Name = "进入出蜡部门")]
        OutputWax = 5,

        [Display(Name = "进入执模部门")]
        Module = 6,

        [Display(Name = "进入打包部门")]
        Pack = 8,

        [Display(Name = "进入待出货阶段")]
        ToBeShip = 9,

        [Display(Name = "出货单生成阶段")]
        Shipmenting = 10,

        [Display(Name = "已出货")]
        Shipment = 11,

        [Display(Name = "已收货")]
        HaveGoods = 12,

        [Display(Name = "进入手镶部门")]
        WithTheHand = 13,

        [Display(Name = "进入微镶部门")]
        MicroInsert = 14,

        [Display(Name = "进入抛光部门")]
        Polishing = 15,

        [Display(Name = "进入倒模部门")]
        DumpModule = 16,

        [Display(Name = "进入等石阶段")]
        WaitStone = 17,

        [Display(Name = "进入主管审核设计图纸阶段")]
        DirectorTobeConfirm = 18,


        [Display(Name = "主管已确认设计")]
        DirectorConfirm = 19,

        [Display(Name = "发送催石消息")]
        CuiShi = 100,

        [Display(Name = "收到客户主石")]
        ReciveStone = 101,

        [Display(Name = "发送确认设计稿消息")]
        CuiQueRen = 102,

        [Display(Name = "主管审核退回设计稿")]
        GobackDesgin = 103,
    }
}
