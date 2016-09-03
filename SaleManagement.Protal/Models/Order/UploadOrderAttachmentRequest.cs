using Dickson.Web.Mvc;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SaleManagement.Protal.Models.Order
{
    public class UploadOrderAttachmentRequest
    {
        [Dickson.Web.Mvc.FileExtensions(ErrorMessage = "上传的{0}只支持图片", Extensions = "jpg,jpeg,gif,png")]
        [Display(Name = "附件"), FileContentLength(MaxSize = SaleManagentConstants.Validations.MaxUploadFileSize)]
        public HttpPostedFileBase File { get; set; }

        public FilePurpose FilePurpose { get; set; }

        public string  OrderId { get; set; }
    }
}