using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public enum SpotGoodsStatus
    {
        [Display(Name = "新建")]
        New = 0 ,

        [Display(Name = "待自取")]
        PickBySelf = 1,

        [Display(Name = "待发货")]
        SF = 2,

        [Display(Name = "已自取")]
        HasTaken = 3,

        [Display(Name = "已发货")]
        HasSendGoods = 4
    }
}
