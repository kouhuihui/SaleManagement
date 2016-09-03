using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class ShipmentOrderInfo
    {
        public ShipmentOrderInfo()
        {
            ProductName = "";
        }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual Order Order { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string ShipmentOrderId { get; set; }

        public virtual ShipmentOrder ShipmentOrder { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string ProductName { get; set; }

        public double Weight { get; set; }

        public double GoldWeight { get; set; }

        public double LossRate { get; set; }

        public double GoldPrice { get; set; }

        public double GoldAmount { get; set; }

        /// <summary>
        /// 风险费
        /// </summary>
        public double RiskFee { get; set; }

        public int SideStoneNumber { get; set; }

        public double SideStoneWeight { get; set; }

        public double TotalSetStoneWorkingCost { get; set; }

        /// <summary>
        /// 副石额
        /// </summary>
        public double SideStoneTotalAmount { get; set; }

        public double BasicCost { get; set; }

        public double OutputWaxCost { get; set; }

        public double OtherCost { get; set; }

        public double TotalAmount { get; set; }
    }
}
