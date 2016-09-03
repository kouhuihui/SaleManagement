using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class OrderAttachment
    {
        public OrderAttachment()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }

        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string FileInfoId { get; set; }

        public DateTime Created { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CreatorId { get; set; }
    }
}
