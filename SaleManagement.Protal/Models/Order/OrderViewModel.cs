using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System.Collections.Generic;

namespace SaleManagement.Protal.Models.Order
{
    public class OrderViewModel : OrderViewModelBase
    {
        public OrderViewModel()
        {
            Attachments = new List<AttachmentItem>();
        }

        public OrderViewModel(SaleManagement.Core.Models.Order order) : base(order)
        {
        }

        public IEnumerable<ProductCategory> ProductCategories { get; set; }

        public IEnumerable<ColorForm> ColorForms { get; set; }

        public IEnumerable<GemCategory> GemCategories { get; set; }

        public IEnumerable<SaleUser> Customers { get; set; }

        public IEnumerable<AttachmentItem> Attachments { get; set; }
    }
}