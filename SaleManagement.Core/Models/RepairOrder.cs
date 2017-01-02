using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
   public class RepairOrder
    {
        public RepairOrder()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string ShipmentOrderId { get; set; }

        public virtual ShipmentOrder ShipmentOrder { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string ProductName { get; set; }

        public double GoldWeight { get; set; }

        public double GoldAmount { get; set; }

        public int StoneNumber { get; set; }

        public double StoneWeight { get; set; }

        public double StoneAmount { get; set; }

        public string Remark { get; set; }

        public double TotalAmount { get; set; }

        public DateTime Created { get; set; }
    }
}
