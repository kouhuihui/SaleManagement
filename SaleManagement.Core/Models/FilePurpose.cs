using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public enum FilePurpose
    {
        [Display(Name = "Order.Attachment")]
        OrderAttachment = 1
    }
}
