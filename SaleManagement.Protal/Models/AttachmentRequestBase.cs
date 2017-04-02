using Dickson.Web.Mvc;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models
{
    public class AttachmentRequestBase
    {
        [Dickson.Web.Mvc.FileExtensions(ErrorMessage = "上传的{0}只支持图片", Extensions = "jpg,jpeg,gif,png")]
        [Display(Name = "附件"), FileContentLength(MaxSize = SaleManagentConstants.Validations.MaxUploadFileSize)]
        public HttpPostedFileBase File { get; set; }

        public FilePurpose FilePurpose { get; set; }
    }
}