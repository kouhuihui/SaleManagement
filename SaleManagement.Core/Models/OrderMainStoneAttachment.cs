using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class OrderMainStoneAttachment
    {
        public OrderMainStoneAttachment()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }

        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        public int OrderMainStoneInfoId { get; set; }

        public virtual OrderMainStoneInfo OrderMainStoneInfo { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string FileInfoId { get; set; }

        public DateTime Created { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CreatorId { get; set; }
    }
}
