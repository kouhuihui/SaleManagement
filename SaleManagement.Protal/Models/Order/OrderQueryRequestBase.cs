using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;

namespace SaleManagement.Protal.Models.Order
{
    public class OrderQueryRequestBase : PagingRequest
    {
        public string CustomerId { get; set; }

        public string OrderId { get; set; }

        public OrderStatus? Status { get; set; }

        public int? ColorFormId { get; set; }

        public string Keyword { get; set; }
    }
}