using AutoMapper;
using Dickson.Core.Common.Extensions;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.HotSelling;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace SaleManagement.Protal.Controllers
{
    public class HotSellingController : PortalController
    {
        // GET: HotSelling
        public async Task<ActionResult> Setting(HotSellingQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new HotSellingManager(User);
            var paging = await manager.GetHotSellingsAsync(request.Start, request.Take, request.GetHotSellingQueryFilter());
            var hotSellingViewModels = paging.List.Select(u => new HotSellingViewModel(u)
            {
            });

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = hotSellingViewModels,
            });
        }

        public async Task<ActionResult> Edit(string id)
        {
            var hotSellingManager = new HotSellingManager(User);
            var model = new HotSellingViewModel();
            var manager = new BasicDataManager(User);

            if (!string.IsNullOrEmpty(id))
            {
                var hotSelling = await hotSellingManager.GetHotSellingAsync(id);

                model = new HotSellingViewModel(hotSelling);
            }

            model.ProductCategories = await manager.GetProductCategoriesAsync();
            model.GemCategories = await manager.GetGemCategoriesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Save(HotSellingCreateViewModel request, [NamedModelBinder(typeof(CommaSeparatedModelBinder), "attachmentIds")] string[] attachmentIds)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            if (string.IsNullOrEmpty(request.Id))
            {
                var serialNumberManager = new SerialNumberManager(User);
                request.Id = Guid.NewGuid().ToString();
                request.VersionNo = SaleManagentConstants.Misc.HotSellingPrefix + await serialNumberManager.NextSNAsync(SaleManagentConstants.SerialNames.HotSelling);
            }

            var hotSelling = Mapper.Map<HotSellingCreateViewModel, HotSelling>(request);
            if (attachmentIds.Any())
            {
                hotSelling.Attachments = new List<HotSellingAttachment>();
                attachmentIds.ForEach<string>(a =>
                    hotSelling.Attachments.Add(new HotSellingAttachment
                    {
                        FileInfoId = a,
                        HotSellingId = hotSelling.Id,
                        CreatorId = User.Id
                    }));
            }
            var manager = new HotSellingManager(User);
            var result = await manager.SaveHotSellingAsync(hotSelling);

            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> Delete(string id)
        {
            var hotSellingManager = new HotSellingManager(User);
            var result = await hotSellingManager.DeleteHotSellingAsync(id);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveHotSelling(string id)
        {
            var manager = new HotSellingManager(User);
            var result = await manager.DeleteHotSellingAsync(id);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddAttachment(UploadHotSellingAttachmentRequest request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary(), contentType: SaleManagentConstants.Misc.JsonResponseContentType);

            Image image = Image.FromStream(request.File.InputStream);

            var file = new Core.Models.FileInfo
            {
                ContentType = request.File.ContentType,
                FileName = request.File.FileName,
                Purpose = request.FilePurpose == 0 ? FilePurpose.HotSellingAttachment.GetDisplayName() : request.FilePurpose.GetDisplayName(),
                Data = ImageHelp.GetByteImage(image),
                ContentLength = request.File.ContentLength
            };

            string strPhysicsPath = HttpContext.Server.MapPath("~/orderImage");

            file.ThumbnailData =
                ImageHelp.MakeThumbnail(request.File.InputStream, 300, 300, strPhysicsPath + "\\" + file.Id + ".jpg").ReadAllBytes();

            image.Dispose();
            var manager = new FileManager(User);
            await manager.CreateAsync(file);
            if (!string.IsNullOrEmpty(request.HotSellingId))
            {
                var hotSellingManager = new HotSellingManager(User);
                var hotSelling = await hotSellingManager.GetHotSellingAsync(request.HotSellingId);
                hotSelling.Attachments.Add(new HotSellingAttachment()
                {
                    FileInfoId = file.Id,
                    HotSellingId = hotSelling.Id,
                    CreatorId = User.Id
                });
                await hotSellingManager.SaveHotSellingAsync(hotSelling);
            }
            return Json(true, data: new { id = file.Id, url = "data:image/jpg;base64," + Convert.ToBase64String(file.ThumbnailData), name = file.FileName, length = file.ThumbnailData.Length },
                   contentType: SaleManagentConstants.Misc.JsonResponseContentType);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveAttachment(string fileId, string hotSellingId = "")
        {
            var hotSellingManager = new HotSellingManager(User);
            if (!string.IsNullOrEmpty(hotSellingId))
            {
                var hotSelling = await hotSellingManager.GetHotSellingAsync(hotSellingId);
                if (hotSelling == null)
                    return Json(SaleManagentConstants.Errors.OrderNotFound);

                var attachement = hotSelling.Attachments.FirstOrDefault(a => a.FileInfoId == fileId);
                if (attachement == null)
                    return Json(false, "不存在此附件");

                await hotSellingManager.RemoveAttachment(attachement);
            }
            var manager = new FileManager(User);
            var result = await manager.DeleteAsync(fileId);
            return Json(result);
        }
    }
}