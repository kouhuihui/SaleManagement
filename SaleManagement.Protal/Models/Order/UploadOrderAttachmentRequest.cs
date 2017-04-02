using Dickson.Web.Mvc;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SaleManagement.Protal.Models.Order
{
    public class UploadOrderAttachmentRequest : AttachmentRequestBase
    {
        public string OrderId { get; set; }
    }
}