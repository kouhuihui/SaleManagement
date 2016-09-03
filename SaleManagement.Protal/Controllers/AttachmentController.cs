using Dickson.Core.ComponentModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class AttachmentController : PortalController
    {
        // GET: Attachment
        [Route("~/Attachment/{fileId}/Preview")]
        public async Task<ActionResult> Preview(string fileId)
        {
            Requires.NotNullOrEmpty(fileId, "fileId");

            var fileManager = new FileManager();
            var file = await fileManager.FindByIdAsync(fileId);
            if (file == null && file.Data != null)
                return Error("文件不存在");

            MemoryStream stream = new MemoryStream(file.Data);
            HttpContext.Response.ContentType = file.ContentType;
            stream.WriteTo(HttpContext.Response.OutputStream);
            return View();
        }

        [Route("~/Attachment/{fileId}/Thumbnail")]
        public async Task<ActionResult> Thumbnail(string fileId)
        {
            Requires.NotNullOrEmpty(fileId, "fileId");

            var fileManager = new FileManager();
            var file = await fileManager.FindByIdAsync(fileId);
            if (file == null && file.Data != null)
                return Error("文件不存在");

            MemoryStream stream = new MemoryStream(file.ThumbnailData);
            HttpContext.Response.ContentType = file.ContentType;
            stream.WriteTo(HttpContext.Response.OutputStream);
            return View();
        }
    }
}