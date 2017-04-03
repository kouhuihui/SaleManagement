using Dickson.Core.Common.Extensions;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models;
using SaleManagement.Protal.Models.SpotGoodsPattern;
using SaleManagement.Protal.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class SpotGoodsPatternController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(PagingRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View(request);

            var manager = new SpotGoodsPatternManager(User);
            var paging = await manager.GetSpotGoodsPatternListAsync(request.Start, request.Take, null);

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = paging.List.Select(r => new SpotGoodsPatternListItemViewModel
                {
                    Id = r.Id,
                    FileInfoId = r.FileInfoId,
                    Name = r.Name,
                    TypeName = r.Type.GetDisplayName()
                })
            });
        }

        public ActionResult Add()
        {
            var model = new SpotGoodsPattern();
            return View(model);
        }

        public async Task<ActionResult> Edit(string Id)
        {
            var manager = new SpotGoodsPatternManager(User);
            var model = await manager.GetSpotGoodsPattern(Id);
            return View("Add", model);
        }

        [HttpPost]
        public async Task<ActionResult> Save(SpotGoodsPattern SpotGoodsPattern)
        {
            if (string.IsNullOrEmpty(SpotGoodsPattern.Id))
            {
                SpotGoodsPattern.Id = Guid.NewGuid().ToString();
            }
            var manager = new SpotGoodsPatternManager(User);
            var result = await manager.SaveSpotGoodsPattern(SpotGoodsPattern);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddAttachment(AttachmentRequestBase request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary(), contentType: SaleManagentConstants.Misc.JsonResponseContentType);

            var file = new Core.Models.FileInfo
            {
                ContentType = request.File.ContentType,
                FileName = request.File.FileName,
                Purpose = FilePurpose.SpotGoodsAttachment.GetDisplayName(),
                Data = await request.File.InputStream.ReadAllBytesAsync(),
                ContentLength = request.File.ContentLength
            };

            var manager = new FileManager(User);
            await manager.CreateAsync(file);

            return Json(true, data: new { id = file.Id, url = "data:image/jpg;base64," + Convert.ToBase64String(file.Data), name = file.FileName, length = file.Data.Length },
                   contentType: SaleManagentConstants.Misc.JsonResponseContentType);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveAttachment(string fileId)
        {
            var manager = new FileManager(User);
            var result = await manager.DeleteAsync(fileId);
            return Json(result);
        }
    }
}