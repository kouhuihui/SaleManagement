using Dickson.Core.Common.Extensions;
using SaleManagement.Core;
using SaleManagement.Core.Models;
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
            Urgent = GetUrgentStatus(order);
            OrderRushStatusName = order.OrderRushStatus.GetDisplayName();
        }

        public string StatusName { get; set; }

        public int Status { get; set; }

        public string CreatorName { get; set; }

        public string CurrentUserName { get; set; }

        public IList<AttachmentItem> Attachments { get; set; }

        public int Urgent { get; set; }

        public string OrderRushStatusName { get; set; }

        private int GetUrgentStatus(Core.Models.Order order)
        {
            var deliveryDate = order.DeliveryDate;
            if (!deliveryDate.HasValue || order.OrderStatus == OrderStatus.Delete || order.OrderStatus == OrderStatus.Shipment || order.OrderStatus == OrderStatus.HaveGoods)
                return (int)UrgentStatus.Normal;

            var now = DateTime.Now.Date;
            var intervalDate = (deliveryDate.Value - now).TotalDays;
            if (intervalDate > SaleManagentConstants.UI.OrderUrgentWaringDay)
                return (int)UrgentStatus.Normal;

            if (intervalDate <= SaleManagentConstants.UI.OrderUrgentWaringDay && intervalDate > SaleManagentConstants.UI.OrderVeryUrgentWaringDay)
                return (int)UrgentStatus.Urgent;

            if (intervalDate <= SaleManagentConstants.UI.OrderVeryUrgentWaringDay)
                return (int)UrgentStatus.VeryUrgent;

            return (int)UrgentStatus.Normal;
        }
    }
}