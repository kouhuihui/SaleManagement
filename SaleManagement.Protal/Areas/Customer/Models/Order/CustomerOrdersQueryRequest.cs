using SaleManagement.Protal.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Areas.Customer.Models.Order
{
    public class CustomerOrdersQueryRequest: OrderQueryRequestBase
    {
        public CustomerOrdersQueryRequest()
        {
            QueryOrderStatus = CustomerQueryOrderStatus.All;
        }

        public CustomerQueryOrderStatus QueryOrderStatus { get; set; }
    }
}