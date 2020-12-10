using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class OrderMainStoneInfo
    {

        public int Id { get; set; }

        public int MainStoneId { get; set; }

        public virtual MainStone MainStone { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        public double Weight { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string Remark { get; set; }

        public DateTime Created { get; set; }

        public string CreatorId { get; set; }

        public virtual ICollection<OrderMainStoneAttachment> Attachments { get; set; }
    }
}
