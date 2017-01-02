using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class ShipmentOrder
    {
        public ShipmentOrder()
        {
            DeliveryDate = DateTime.Now;
            Created = DateTime.Now;
            AuditStatus = ShipmentOrderAduitStatus.New;
        }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int CompanyId { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int TotalNumber { get; set; }

        public double TotalWeight { get; set; }

        public double TotalGoldWeight { get; set; }

        public double TotalAmount { get; set; }

        public string CreatorId { get; set; }

        public string CreatorName { get; set; }

        public DateTime Created { get; set; }

        public string AuditorName { get; set; }

        public DateTime? AuditDate { get; set; }

        public ShipmentOrderAduitStatus AuditStatus { get; set; }

        public virtual ICollection<ShipmentOrderInfo> ShipmentOrderInfos { get; set; }

        public virtual ICollection<RepairOrder> RepairOrders { get; set; }
    }
}
