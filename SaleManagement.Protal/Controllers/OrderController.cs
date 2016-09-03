using AutoMapper;
using Dickson.Core.Common.Extensions;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Order;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace SaleManagement.Protal.Controllers
{
    [RoutePrefix("order")]
    public class OrderController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(OrdersQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new OrderManager(User);

            var paging = await manager.GetOrdersAsync(request.Start, request.Take, request.GetOrderListQueryFilter(User));
            var orders = paging.List.Select(u =>
            {
                return new OrderListItemViewModel(u);
            });

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = orders,
            });
        }

        [PagingParameterInspector]
        public async Task<ActionResult> MyOrders(PagingRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new OrderManager(User);
            Func<IQueryable<Order>, IQueryable<Order>> filter = query =>
            {
                query = query.Where(j => j.CurrentUserId == User.Id);
                return query;
            };
            var paging = await manager.GetOrdersAsync(request.Start, request.Take, filter);
            var basicDataManager = new BasicDataManager(User);
            var colorForms = await basicDataManager.GetColorFormsAsync();
            var orders = paging.List.Select(u =>
            {
                var colorForm = colorForms.FirstOrDefault(f => f.Id == u.ColorFormId);
                return new OrderListItemViewModel(u)
                {
                    ColorFormName = colorForm == null ? "" : colorForm.Name
                };
            });

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = orders,
            });
        }

        public async Task<ActionResult> Booking()
        {
            var model = new OrderViewModel();
            var manager = new BasicDataManager(User);
            model.ProductCategories = await manager.GetProductCategoriesAsync();
            model.ColorForms = await manager.GetColorFormsAsync();
            model.GemCategories = await manager.GetGemCategoriesAsync();
            var customers = await new UserManager().GetAllCustomersAsync();
            model.Customers = customers;
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Booking(OrderEditViewModel request, [NamedModelBinder(typeof(CommaSeparatedModelBinder), "attachmentIds")] string[] attachmentIds)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var order = Mapper.Map<OrderEditViewModel, Order>(request);
            var serialNumberManager = new SerialNumberManager(User);

            order.Id = SaleManagentConstants.Misc.OrderPrefix + await serialNumberManager.NextSNAsync(SaleManagentConstants.SerialNames.Order);

            if (attachmentIds.Any())
            {
                order.Attachments = new List<OrderAttachment>();
                attachmentIds.ForEach<string>(a =>
                    order.Attachments.Add(new OrderAttachment
                    {
                        FileInfoId = a,
                        OrderId = order.Id,
                        CreatorId = User.Id
                    }));
            }
            var manager = new OrderManager(User);
            var result = await manager.CreateOrder(order);
            if (result.Succeeded)
            {
                var operationLogManager = new OrderOperationLogManager(User);
                await operationLogManager.AddLogAsync(OrderStatus.UnConfirmed, order.Id);

                var noticeManager = new NoticeManager(User);
                var notice = await noticeManager.GetNewNoticeAsync();
                return Json(result.Succeeded, data:
                    new
                    {
                        orderId = order.Id,
                        notice = notice.Content
                    });
            }

            return Json(result);
        }

        public async Task<ActionResult> Edit(string orderId)
        {
            var orderManager = new OrderManager(User);
            var order = await orderManager.GetOrderAsync(orderId);
            if (order == null)
                return Json(false, SaleManagentConstants.Errors.OrderNotFound);

            var model = new OrderViewModel(order);
            var manager = new BasicDataManager(User);
            model.ProductCategories = await manager.GetProductCategoriesAsync();
            model.ColorForms = await manager.GetColorFormsAsync();
            model.GemCategories = await manager.GetGemCategoriesAsync();
            var customers = await new UserManager().GetAllCustomersAsync();
            model.Customers = customers;
            model.Attachments = await Task.WhenAll(order.Attachments.Select(async a =>
            {
                var fileManager = new FileManager();
                var file = await fileManager.FindByIdAsync(a.FileInfoId);
                return new AttachmentItem
                {
                    Id = a.FileInfoId,
                    Name = file.FileName,
                    Length = file.ContentLength,
                    Url = "data:image/jpg;base64," + Convert.ToBase64String(file.Data)
                };
            }).ToList());

            return View("Booking", model);
        }

        [HttpPost]
        public async Task<JsonResult> Edit(OrderEditViewModel request)
        {
            var orderManager = new OrderManager(User);
            var order = await orderManager.GetOrderAsync(request.Id);
            if (order == null)
                return Json(false, SaleManagentConstants.Errors.OrderNotFound);

            order.DeliveryDate = Convert.ToDateTime(request.DeliveryDate);
            order.GemCategoryId = request.GemCategoryId;
            order.ProductCategoryId = request.ProductCategoryId;
            order.ColorFormId = request.ColorFormId;
            order.Number = request.Number;
            order.MainStoneSize = request.MainStoneSize;
            order.Certificate = request.Certificate;
            order.WordsPrinted = request.WordsPrinted;
            order.GoldWeightRequirement = request.GoldWeightRequirement;
            order.SideStoneRequiredment = request.SideStoneRequiredment;
            order.RadianRequirement = request.RadianRequirement;
            order.RabbetRequirement = request.RabbetRequirement;
            order.StoneDescribe = request.StoneDescribe;
            order.HasOldMaterial = request.HasOldMaterial;
            order.Remark = request.Remark;
            order.Updated = DateTime.Now;
            order.MinChainLength = request.MinChainLength;
            order.MaxChainLength = request.MaxChainLength;
            order.HandSize = request.HandSize;
            var result = await orderManager.UpdateOrderAsync(order);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddAttachment(UploadOrderAttachmentRequest request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary(), contentType: SaleManagentConstants.Misc.JsonResponseContentType);

            var file = new FileInfo
            {
                ContentType = request.File.ContentType,
                FileName = request.File.FileName,
                Purpose = FilePurpose.OrderAttachment.GetDisplayName(),
                Data = await request.File.InputStream.ReadAllBytesAsync(),
                ContentLength = request.File.ContentLength
            };

            var manager = new FileManager(User);
            await manager.CreateAsync(file);
            if (!string.IsNullOrEmpty(request.OrderId))
            {
                var orderManager = new OrderManager(User);
                var order = await orderManager.GetOrderAsync(request.OrderId);
                order.Attachments.Add(new OrderAttachment
                {
                    FileInfoId = file.Id,
                    OrderId = order.Id,
                    CreatorId = User.Id
                });
                await orderManager.UpdateOrderAsync(order);
            }
            return Json(true, data: new { id = file.Id, url = "data:image/jpg;base64," + Convert.ToBase64String(file.Data), name = file.FileName, length = file.ContentLength },
                   contentType: SaleManagentConstants.Misc.JsonResponseContentType);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveAttachment(string fileId, string orderId = "")
        {
            var orderManager = new OrderManager(User);
            if (!string.IsNullOrEmpty(orderId))
            {
                var order = await orderManager.GetOrderAsync(orderId);
                if (order == null)
                    return Json(SaleManagentConstants.Errors.OrderNotFound);
                var attachement = order.Attachments.FirstOrDefault(a => a.FileInfoId == fileId);
                if (attachement == null)
                    return Json(false, "不存在此附件");
                await orderManager.RemoveAttachment(attachement);
            }
            var manager = new FileManager(User);
            var result = await manager.DeleteAsync(fileId);
            return Json(result);
        }

        [Route("{orderId}/Distribution")]
        public ActionResult DistributionOrder(string orderId)
        {
            var model = new DistributionOrderViewModel();
            model.OrderId = orderId;
            return View(model);
        }

        [HttpPost, Route("{orderId}/Distribution")]
        public async Task<JsonResult> DistributionOrder(string orderId, ModuleType moduleType, string userId)
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            order.ModuleType = moduleType;
            order.CurrentUserId = userId;

            if (order.ModuleType == ModuleType.Jina || order.ModuleType == ModuleType.Customer)
            {
                order.OrderStatus = OrderStatus.OutputWax;
            }
            else
            {
                order.OrderStatus = OrderStatus.Design;
            }
            var result = await manager.UpdateOrderAsync(order);
            if (result.Succeeded)
            {
                var operationLogManager = new OrderOperationLogManager(User);
                await operationLogManager.AddLogAsync(order.OrderStatus, order.Id);
            }
            return Json(result);
        }

        [HttpPost, Route("{orderId}/CustomerTobeConfirm")]
        public async Task<JsonResult> GotoCustomerTobeConfirmStep(string orderId, double outputWaxCost = 0)
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            if (order == null)
                return Json(false, SaleManagentConstants.Errors.OrderNotFound);

            var designImages = order.Attachments.Where(r => r.CreatorId != order.CreatorId);
            if (!designImages.Any())
                return Json(false, "设计师还未上传设计图，不能进入客户确认阶段。");

            order.CurrentUserId = "";
            order.OrderStatus = OrderStatus.CustomerTobeConfirm;
            order.OutputWaxCost = outputWaxCost;
            var result = await manager.UpdateOrderAsync(order);
            if (result.Succeeded)
            {
                var operationLogManager = new OrderOperationLogManager(User);
                await operationLogManager.AddLogAsync(order.OrderStatus, order.Id);
            }
            return Json(result);
        }

        [Route("{orderId}/NextStep")]
        public ActionResult NextStep(string orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        [HttpPost, Route("{orderId}/GoNextStep")]
        public async Task<JsonResult> GoNextStep(string orderId, OrderStatus nextStatus, string userId)
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            if (order == null)
                return Json(false, SaleManagentConstants.Errors.OrderNotFound);

            order.CurrentUserId = userId;
            order.OrderStatus = nextStatus;
            var result = await manager.UpdateOrderAsync(order);
            if (result.Succeeded)
            {
                var operationLogManager = new OrderOperationLogManager(User);
                await operationLogManager.AddLogAsync(order.OrderStatus, order.Id);
            }

            return Json(result);
        }

        [Route("{orderId}/upload/desginimage")]
        public async Task<ActionResult> UploadDesginImage(string orderId)
        {
            var orderManager = new OrderManager(User);
            var order = await orderManager.GetOrderAsync(orderId);
            var attachements = order.Attachments.Where(a => a.CreatorId == User.Id);
            return View(new UploadDesginImageViewModel()
            {
                OrderId = orderId,
                Attachments = await Task.WhenAll(attachements.Select(async a =>
                {
                    var fileManager = new FileManager();
                    var file = await fileManager.FindByIdAsync(a.FileInfoId);
                    return new AttachmentItem
                    {
                        Id = a.FileInfoId,
                        Name = file.FileName,
                        Length = file.ContentLength,
                        Url = "data:image/jpg;base64," + Convert.ToBase64String(file.Data)
                    };
                }).ToList())
            });
        }

        public async Task<ActionResult> Detail(string orderId)
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            var orderListItemViewModel = new OrderListItemViewModel(order);
            var attachements = order.Attachments;
            orderListItemViewModel.Attachments = await Task.WhenAll(attachements.Select(async a =>
             {
                 var fileManager = new FileManager();
                 var file = await fileManager.FindByIdAsync(a.FileInfoId);
                 return new AttachmentItem
                 {
                     Id = a.FileInfoId,
                     Name = file.FileName,
                     Length = file.ContentLength,
                     Url = "data:image/jpg;base64," + Convert.ToBase64String(file.Data)
                 };
             }).ToList());
            return View(orderListItemViewModel);
        }

        public async Task<ActionResult> SetStone(string orderId)
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            var orderSetStoneViewModel = new OrderSetStoneViewModel(order);
            return View(orderSetStoneViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SetStone(OrderSetStoneViewModel request)
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(request.Id);
            if (order == null)
                return Json(false, SaleManagentConstants.Errors.OrderNotFound);

            order.MainStoneNumber = request.MainStoneNumber;
            order.MainStoneSize = request.MainStoneSize;
            var result = await manager.UpdateOrderAsync(order);
            return Json(result);
        }

        public async Task<ActionResult> AddSetStone(string orderId)
        {
            var manager = new BasicDataManager();
            var matchStones = await manager.GetMatchStonesAsync();
            var orderSetStoneInfoViewModel = new OrderSetStoneInfoViewModel
            {
                OrderId = orderId,
                MatchStones = matchStones
            };
            return View(orderSetStoneInfoViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddSetStone(OrderSetStoneInfoViewModel request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var orderSetStoneInfo = Mapper.Map<OrderSetStoneInfoViewModel, OrderSetStoneInfo>(request);
            var basicDataManager = new BasicDataManager();
            var matchStone = await basicDataManager.GetMatchStoneAsync(request.MatchStoneId);
            orderSetStoneInfo.CreatorId = User.Id;
            orderSetStoneInfo.MathchStoneName = matchStone.Name;
            orderSetStoneInfo.Price = matchStone.Price;
            orderSetStoneInfo.WorkingCost = matchStone.WorkingCost * orderSetStoneInfo.Number;

            var orderSetStoneInfoManager = new OrderSetStoneInfoManager();
            var result = await orderSetStoneInfoManager.CreateOrderSetStoneInfoAsync(orderSetStoneInfo);
            return Json(result);
        }
    }
}