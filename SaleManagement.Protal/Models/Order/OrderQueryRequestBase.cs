using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Order
{
    public  class OrderQueryRequestBase : PagingRequest
    {
        public string CustomerId { get; set; }

        public string OrderId { get; set; }

        public OrderStatus? Status { get; set; }

        public int? ColorFormId { get; set; }
    }
}