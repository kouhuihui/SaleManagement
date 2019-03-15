using SaleManagement.Core.Models;
using System.Collections.Generic;

namespace SaleManagement.Protal.Models.HotSelling
{
    public class HotSellingCreateViewModel : HotSellingBase
    {
        public virtual ICollection<HotSellingAttachment> Attachments { get; set; }
    }
}