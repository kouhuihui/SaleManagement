using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Order
{
    public class UploadDesginImageViewModel
    {
        public string OrderId { get; set; }

        public IList<SaleManagement.Core.ViewModel.AttachmentItem> Attachments { get; set; }
    }
}