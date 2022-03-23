using AutoMapper;
using Dickson.Core.Common.Extensions;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Protal.Models;
using SaleManagement.Protal.Models.SpotGoodsPattern;
using SaleManagement.Protal.Web;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class SpotGoodsPatternController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(SpotGoodPatternPageRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View(request);

            var manager = new SpotGoodsPatternManager(User);
            var paging = await manager.GetSpotGoodsPatternListAsync(request.Start, request.Take, request.GetSpotGoodsPatternListQueryFilter(User));

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = paging.List.Select(r => new SpotGoodsPatternListItemViewModel
                {
                    Id = r.Id,
                    FileInfoId = r.FileInfoId,
                    Name = r.Name,
                    //TypeName = r.SpotGoodType.Name,
                    Price = r.Price,
                    Url = "/Attachment/" + r.FileInfoId + "/Thumbnail"
                })
            });
        }

        public async Task<ActionResult> Add()
        {
            var model = new SpotGoodsPatternEditViewModel();
            var manager = new SpotGoodTypeManager(User);
            model.SpotGoodTypes = await manager.GetSpotGoodTypeListAsync();

            var basicDataManager = new BasicDataManager(User);
            model.ProductCategories = await basicDataManager.GetProductCategoriesAsync();
            model.GemCategories = await basicDataManager.GetGemCategoriesAsync();
            return View(model);
        }

        public async Task<ActionResult> Edit(string Id)
        {
            var manager = new SpotGoodsPatternManager(User);
            var spotGoodsPattern = await manager.GetSpotGoodsPattern(Id);
            var model = Mapper.Map<SpotGoodsPattern, SpotGoodsPatternEditViewModel>(spotGoodsPattern);

            var spotGoodTypeManager = new SpotGoodTypeManager(User);
            model.SpotGoodTypes = await spotGoodTypeManager.GetSpotGoodTypeListAsync();

            var basicDataManager = new BasicDataManager(User);
            model.ProductCategories = await basicDataManager.GetProductCategoriesAsync();
            model.GemCategories = await basicDataManager.GetGemCategoriesAsync();
            model.SpotGoodsPatternTypeIds =
                spotGoodsPattern.SpotGoodsPatternTypes.Select(t => t.SpotGoodsTypeId).ToList();
            return View("Add", model);
        }

        [HttpPost]
        public async Task<ActionResult> Save(SpotGoodsPattern spotGoodsPattern, [NamedModelBinder(typeof(CommaSeparatedModelBinder), "SpotGoodsPatternTypeIdStr")] string[] spotGoodsPatternTypeIdStr)
        {
            if (string.IsNullOrEmpty(spotGoodsPattern.Id))
            {
                spotGoodsPattern.Id = Guid.NewGuid().ToString();
            }
            var manager = new SpotGoodsPatternManager(User);
            var result = await manager.EditSpotGoodsPattern(spotGoodsPattern, spotGoodsPatternTypeIdStr);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddAttachment(AttachmentRequestBase request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary(), contentType: SaleManagentConstants.Misc.JsonResponseContentType);

            Image image = Image.FromStream(request.File.InputStream);

            var file = new Core.Models.FileInfo
            {
                ContentType = request.File.ContentType,
                FileName = request.File.FileName,
                Purpose = FilePurpose.SpotGoodsAttachment.GetDisplayName(),
                Data = await request.File.InputStream.ReadAllBytesAsync(),
                ContentLength = request.File.ContentLength
            };

            string strPhysicsPath = HttpContext.Server.MapPath("~/orderImage");

            file.ThumbnailData =
                ImageHelp.MakeThumbnail(request.File.InputStream, 300, 300, strPhysicsPath + "\\" + file.Id + ".jpg").ReadAllBytes();

            image.Dispose();

            var manager = new FileManager(User);
            await manager.CreateAsync(file);

            return Json(true, data: new { id = file.Id, url = "data:image/jpg;base64," + Convert.ToBase64String(file.ThumbnailData), name = file.FileName, length = file.Data.Length },
                   contentType: SaleManagentConstants.Misc.JsonResponseContentType);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveAttachment(string fileId)
        {
            var manager = new FileManager(User);
            var result = await manager.DeleteAsync(fileId);
            return Json(result);
        }

        public async Task<JsonResult> GetSpotGoodsPatterns(string typeId)
        {
            var manager = new SpotGoodsPatternManager(User);
            var result = await manager.GetSpotGoodsPatternListAsync(typeId);
            return Json(true, string.Empty, result);
        }
    }
}