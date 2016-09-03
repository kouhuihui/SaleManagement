using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Order
{
    public enum CustomerQueryOrderStatus
    {
        All = 0,
        Process = 1,
        ForGoods = 2,
        HaveGoods = 3
    }
}