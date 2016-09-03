using Dickson.Core.Common.Extensions;
using SaleManagement.Core;
using SaleManagement.Core.ViewModel;
using System.Collections.Generic;

namespace SaleManagement.Protal.Models.Order
{
    public class OrderListItemViewModel : OrderViewModelBase
    {
        public OrderListItemViewModel(SaleManagement.Core.Models.Order order):base(order)
        {           
            StatusName = order.OrderStatus.GetDisplayName();
            Status = (int)order.OrderStatus;
            CreatorName = order.CreatorName;
        }

        public string StatusName { get; set; }

        public int Status { get; set; }

        public string CreatorName { get; set; }

        public IList<AttachmentItem> Attachments { get; set; }
    }
}