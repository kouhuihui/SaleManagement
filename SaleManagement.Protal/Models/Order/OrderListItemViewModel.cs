using Dickson.Core.Common.Extensions;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections.Generic;

namespace SaleManagement.Protal.Models.Order
{
    public class OrderListItemViewModel : OrderViewModelBase
    {
        public OrderListItemViewModel(SaleManagement.Core.Models.Order order) : base(order)
        {
            StatusName = order.OrderStatus.GetDisplayName();
            Status = (int)order.OrderStatus;
            CreatorName = order.CreatorName;
            UrgentStatus = GetUrgentStatus(order.DeliveryDate);
        }

        public string StatusName { get; set; }

        public int Status { get; set; }

        public string CreatorName { get; set; }

        public string CurrentUserName { get; set; }

        public IList<AttachmentItem> Attachments { get; set; }

        public UrgentStatus UrgentStatus { get; set; }

        private UrgentStatus GetUrgentStatus(DateTime? deliveryDate)
        {
            if (!deliveryDate.HasValue)
                return UrgentStatus.Normal;

            var now = DateTime.Now.Date;
            var intervalDate = (deliveryDate.Value - now).Days;
            if (intervalDate > 6)
                return UrgentStatus.Normal;

            if (intervalDate <= 6 && intervalDate > 3)
                return UrgentStatus.Urgent;

            if (intervalDate <= 3)
                return UrgentStatus.VeryUrgent;

            return UrgentStatus.Normal;
        }
    }
}