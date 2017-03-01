using AutoMapper;
using Dickson.Core.Common.Extensions;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.SpotGoods;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace SaleManagement.Protal.Controllers
{
    public class SpotGoodsController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(SpotGoodsQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View(request);

            var manager = new SpotGoodsManager(User);
            var paging = await manager.GetSpotGoodsAsync(request.Start, request.Take, null);

            var spotGoodList = paging.List.Select(u => {
                var spotGoods = Mapper.Map<SpotGoods, SpotGoodsBase>(u);
                return spotGoods;
            });
            return Json(true, string.Empty, new
            {
                paging.Total,
                List = spotGoodList
            });
        }

        public ActionResult Create()
        {
            var spotGoodViewModel = new SpotGoodsViewModel();
            return View(spotGoodViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(SpotGoodsEditViewModel  request, [NamedModelBinder(typeof(CommaSeparatedModelBinder), "attachmentIds")] string[] attachmentIds)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var spotGoods = Mapper.Map<SpotGoodsEditViewModel, SpotGoods>(request);

            var serialNumberManager = new SerialNumberManager(User);
            var manager = new SpotGoodsManager(User);
            spotGoods.Id = SaleManagentConstants.Misc.SpotGoodsPrefix + await serialNumberManager.NextSNAsync(SaleManagentConstants.SerialNames.SpotGoods);

            if (attachmentIds.Any())
            {
                spotGoods.Attachments = new List<SpotGoodsAttachment>();
                attachmentIds.ForEach<string>(a =>
                    spotGoods.Attachments.Add(new SpotGoodsAttachment
                    {
                        FileInfoId = a,
                        SpotGoodsId = spotGoods.Id,
                        CreatorId = User.Id
                    }));
            }
            var result = await manager.CreateSpotGoods(spotGoods);
            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> AddAttachment(SpotGoodsAttachmentRequest request)
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
            if (!string.IsNullOrEmpty(request.SpotGoodsId))
            {
                //var spotGoodsManager = new SpotGoodsManager(User);
                //var order = await spotGoodsManager.GetOrderAsync(request.SpotGoodsId);
                //order.Attachments.Add(new OrderAttachment
                //{
                //    FileInfoId = file.Id,
                //    OrderId = order.Id,
                //    CreatorId = User.Id
                //});
                //await spotGoodsManager.UpdateOrderAsync(order);
            }
            return Json(true, data: new { id = file.Id, url = "data:image/jpg;base64," + Convert.ToBase64String(file.Data), name = file.FileName, length = file.Data.Length },
                   contentType: SaleManagentConstants.Misc.JsonResponseContentType);
        }
    }
}