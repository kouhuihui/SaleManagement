using SaleManagement.Core;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Shipment
{
    public class ShipmentOrderViewModel
    {
        public ShipmentOrderViewModel()
        {
            DeliveryDate = DateTime.Now.ToString(SaleManagentConstants.UI.DateStringFormat);
        }

        public string Id { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string DeliveryDate { get; set; }

        public int TotalNumber { get; set; }

        public double TotalWeight { get; set; }

        public double TotalGoldWeight { get; set; }

        public double TotalAmount { get; set; }

        public string CreatorName { get; set; }

        public string Created { get; set; }

        public string AuditorName { get; set; }

        public string AuditDate { get; set; }

        public int AuditStatus { get; set; }

        public IEnumerable<ShipmentOrderInfoViewModel> ShipmentOrderInfos { get; set; }
    }
}