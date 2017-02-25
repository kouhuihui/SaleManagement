using AutoMapper;
using Dickson.Core.Common.Extensions;
using Dickson.Core.ComponentModel;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Order;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace SaleManagement.Protal.Controllers
{
    [RoutePrefix("order")]
    public class OrderController : PortalController
    {
        [UrlAuthorize]
        [PagingParameterInspector]
        public async Task<ActionResult> List(OrdersQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View(request);

            var manager = new OrderManager(User);
            var paging = await manager.GetOrdersAsync(request.Start, request.Take, request.GetOrderListQueryFilter(User), request.OutPutWaxDate);

            var users = await new UserManager().GetUsersAsync(paging.List.Select(o => o.CurrentUserId));
            var orders = paging.List.Select(u => new OrderListItemViewModel(u)
            {
                CurrentUserName = users.FirstOrDefault(s => s.Id == u.CurrentUserId)?.Name
            });

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = orders,
            });
        }

        [UrlAuthorize]
        [PagingParameterInspector]
        public async Task<ActionResult> MyOrders(OrdersQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new OrderManager(User);

            var paging = await manager.GetDesignOrdersAsync(request.Start, request.Take, request.GetOrderListQueryFilter(User));
            var basicDataManager = new BasicDataManager(User);
            var colorForms = await basicDataManager.GetColorFormsAsync();

            var designs = await new UserManager().GetUserByRoleAsync(SaleManagentConstants.SystemRole.Design);

            var orders = paging.List.Select(u =>
            {
                var colorForm = colorForms.FirstOrDefault(f => f.Id == u.ColorFormId);
                return new OrderListItemViewModel(u)
                {
                    ColorFormName = colorForm == null ? "" : colorForm.Name,
                    CurrentUserName = designs.FirstOrDefault(s => s.Id == u.CurrentUserId)?.Name,
                    Attachments = u.Attachments.Where(r => designs.Any(d => d.Id == r.CreatorId)).Select(a => new AttachmentItem
                    {
                        Id = a.FileInfoId,
                        Url = "/Attachment/" + a.FileInfoId + "/Thumbnail"
                    }).ToList()
                };
            });

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = orders,
            });
        }

        [HttpPost]
        public async Task<JsonResult> AssginToMe([NamedModelBinder(typeof(CommaSeparatedModelBinder), "orderIds")] string[] orderIds)
        {
            var manager = new OrderManager(User);
            var orders = await manager.GetOrdersAsync(orderIds);

            foreach (var order in orders)
            {
                order.CurrentUserId = User.Id;
            }
            var result = await manager.UpdateOrdersAsync(orders);
            return Json(result);
        }

        public async Task<ActionResult> Booking()
        {
            var model = new OrderViewModel();
            var manager = new BasicDataManager(User);
            model.ProductCategories = await manager.GetProductCategoriesAsync();
            model.ColorForms = await manager.GetColorFormsAsync();
            model.GemCategories = await manager.GetGemCategoriesAsync();

            var shippingScheduleDays = await manager.GetShippingScheduleDaysAsync();
            model.Created = DateTime.Now.ToString(SaleManagentConstants.UI.DateStringFormat);
            model.DeliveryDate = DateTime.Now.AddDays(shippingScheduleDays).ToString(SaleManagentConstants.UI.DateStringFormat);
            var customers = await new UserManager().GetAllCustomersAsync();
            model.Customers = customers;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Booking(OrderEditViewModel request, [NamedModelBinder(typeof(CommaSeparatedModelBinder), "attachmentIds")] string[] attachmentIds)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var order = Mapper.Map<OrderEditViewModel, Order>(request);

            var shippingScheduleDays = await new BasicDataManager(User).GetShippingScheduleDaysAsync();
            SetOrderDeliveryDate(order, shippingScheduleDays);

            var serialNumberManager = new SerialNumberManager(User);
            var manager = new OrderManager(User);
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
            var result = await manager.CreateOrder(order);
            if (result.Succeeded)
            {
                var operationLogManager = new OrderOperationLogManager(User);
                await operationLogManager.AddLogAsync(OrderStatus.UnConfirmed, order.Id);

                return Json(result.Succeeded, data:
                    new
                    {
                        orderId = order.Id,
                        notice = $"最近出货期限{shippingScheduleDays}天"
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
            model.Attachments = order.Attachments.OrderByDescending(a => a.Created).Select(a => new AttachmentItem
            {
                Id = a.FileInfoId,
                Url = "/Attachment/" + a.FileInfoId + "/Thumbnail"
            }).ToList();

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
            order.OrderRushStatus = request.OrderRushStatus;
            var shippingScheduleDays = await new BasicDataManager(User).GetShippingScheduleDaysAsync();
            SetOrderDeliveryDate(order, shippingScheduleDays);
            var result = await orderManager.UpdateOrderAsync(order);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddAttachment(UploadOrderAttachmentRequest request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary(), contentType: SaleManagentConstants.Misc.JsonResponseContentType);

            Image image = Image.FromStream(request.File.InputStream);

            var file = new Core.Models.FileInfo
            {
                ContentType = request.File.ContentType,
                FileName = request.File.FileName,
                Purpose = FilePurpose.OrderAttachment.GetDisplayName(),
                Data = GetByteImage(image),
                ThumbnailData = MakeThumbnail(request.File.InputStream, 300, 300).ReadAllBytes(),
                ContentLength = request.File.ContentLength
            };

            image.Dispose();
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
            return Json(true, data: new { id = file.Id, url = "data:image/jpg;base64," + Convert.ToBase64String(file.ThumbnailData), name = file.FileName, length = file.ThumbnailData.Length },
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

        public ActionResult DistributionOrder(string orderIds)
        {
            var model = new DistributionOrderViewModel();
            model.OrderIds = orderIds;
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> DistributionOrder([NamedModelBinder(typeof(CommaSeparatedModelBinder), "orderIds")] string[] orderIds, ModuleType moduleType)
        {
            OrderStatus orderStatus;
            if (moduleType == ModuleType.Jina || moduleType == ModuleType.Customer)
            {
                orderStatus = OrderStatus.OutputWax;
            }
            else
            {
                orderStatus = OrderStatus.Design;
            }
            var manager = new OrderManager(User);
            var orders = await manager.GetOrdersAsync(orderIds);

            foreach (var order in orders)
            {
                order.ModuleType = moduleType;
                order.OrderStatus = orderStatus;
            }
            var result = await manager.UpdateOrdersAsync(orders);
            if (result.Succeeded)
            {
                var operationLogManager = new OrderOperationLogManager(User);
                await operationLogManager.AddLogAsync(orderStatus, orderIds);
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

        [HttpPost, Route("{orderId}/SetOutputwaxCost")]
        public async Task<JsonResult> SetOutputwaxCost(string orderId, double outputWaxCost = 0)
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            if (order == null)
                return Json(false, SaleManagentConstants.Errors.OrderNotFound);

            order.OutputWaxCost = outputWaxCost;
            var result = await manager.UpdateOrderAsync(order);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GotoOutputWax([NamedModelBinder(typeof(CommaSeparatedModelBinder), "orderIds")] string[] orderIds)
        {
            var result = await ChangeStep(orderIds, OrderStatus.OutputWax);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GotoDumpModule([NamedModelBinder(typeof(CommaSeparatedModelBinder), "orderIds")] string[] orderIds)
        {
            var result = await ChangeStep(orderIds, OrderStatus.DumpModule);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GotoWaitStone([NamedModelBinder(typeof(CommaSeparatedModelBinder), "orderIds")] string[] orderIds)
        {
            var result = await ChangeStep(orderIds, OrderStatus.WaitStone);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Delete([NamedModelBinder(typeof(CommaSeparatedModelBinder), "orderIds")] string[] orderIds)
        {
            var result = await ChangeStep(orderIds, OrderStatus.Delete);
            return Json(result);
        }

        public async Task<ActionResult> Stop(string orderId)
        {
            Requires.NotNullOrEmpty(orderId, "orderId");

            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            if (order == null)
                return View("Error", SaleManagentConstants.Errors.OrderNotFound);

            StopOrderViewModel stopOrderViewModel = new StopOrderViewModel
            {
                OrderId = orderId,
                DesginAmount = order.OutputWaxCost,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name
            };
            return View(stopOrderViewModel);
        }

        /// <summary>
        /// 消单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Stop(StopOrderViewModel stopOrderViewModel)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var result = await ChangeStep(new string[] { stopOrderViewModel.OrderId }, OrderStatus.Delete);

            if (stopOrderViewModel.DesginAmount <= 0 && stopOrderViewModel.OtherAmount <= 0)
                return Json(result);

            var manager = new ReconciliationManager(User);
            var reconciliation = new Reconciliation
            {
                CustomerId = stopOrderViewModel.CustomerId,
                CustomerName = stopOrderViewModel.CustomerName,
                Amount = stopOrderViewModel.DesginAmount + stopOrderViewModel.OtherAmount,
                CompanyId = User.CompanyId,
                Type = ReconciliationType.Arrearage,
                Created = DateTime.Now,
                CreatorId = User.Id,
                Remark = $"{stopOrderViewModel.OrderId}消单," + (stopOrderViewModel.DesginAmount > 0 ? $"设计费用{stopOrderViewModel.DesginAmount}," : "")
                + (stopOrderViewModel.OtherAmount > 0 ? $"其他费用{ stopOrderViewModel.OtherAmount}" : "")
            };
            await manager.CreateAsync(reconciliation);

            return Json(result);
        }

        private async Task<InvokedResult> ChangeStep(string[] orderIds, OrderStatus orderStatus, string userId = "")
        {
            var manager = new OrderManager(User);
            var orders = await manager.GetOrdersAsync(orderIds);

            foreach (var order in orders)
            {
                order.CurrentUserId = userId;
                order.OrderStatus = orderStatus;
            }
            var result = await manager.UpdateOrdersAsync(orders);
            if (result.Succeeded)
            {
                var operationLogManager = new OrderOperationLogManager(User);
                await operationLogManager.AddLogAsync(orderStatus, orderIds);
            }
            return result;
        }

        public ActionResult NextStep(string orderIds)
        {
            ViewBag.OrderIds = orderIds;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> NextStep([NamedModelBinder(typeof(CommaSeparatedModelBinder), "orderIds")] string[] orderIds, OrderStatus nextStatus, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Json(false, "请选择处理人");

            var result = await ChangeStep(orderIds, nextStatus, userId);
            return Json(result);
        }

        [Route("{orderId}/upload/desginimage")]
        public async Task<ActionResult> UploadDesginImage(string orderId)
        {
            var orderManager = new OrderManager(User);
            var order = await orderManager.GetOrderAsync(orderId);
            var desginers = await new UserManager().GetUserByRoleAsync(SaleManagentConstants.SystemRole.Design);
            var attachements = order.Attachments.Where(a => desginers.Any(d => d.Id == a.CreatorId));
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
                        Length = file.ThumbnailData.Length,
                        Url = "data:image/jpg;base64," + Convert.ToBase64String(file.ThumbnailData)
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
            orderListItemViewModel.Attachments = attachements.OrderByDescending(a => a.Created).Take(4).Select(a => new AttachmentItem
            {
                Id = a.FileInfoId,
                Url = "/Attachment/" + a.FileInfoId + "/Thumbnail"
            }).ToList();
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


        [HttpPost]
        public async Task<JsonResult> DeleteSetStone(int id)
        {
            var orderSetStoneInfoManager = new OrderSetStoneInfoManager();
            var result = await orderSetStoneInfoManager.DeleteOrderSetStoneAsync(id);
            return Json(result);
        }

        public async Task<ActionResult> Pack(string orderId)
        {
            var model = new OrderPackViewModel();
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            model.OrderId = order.Id;
            model.GoldWeight = order.GoldWeight;
            model.Weight = order.Weight;
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Pack(OrderPackViewModel viewModel)
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(viewModel.OrderId);
            order.Weight = viewModel.Weight;
            order.GoldWeight = viewModel.GoldWeight;
            order.OrderStatus = OrderStatus.ToBeShip;
            var result = await manager.UpdateOrderAsync(order);
            if (result.Succeeded)
            {
                var operationLogManager = new OrderOperationLogManager(User);
                await operationLogManager.AddLogAsync(order.OrderStatus, order.Id);
            }
            return Json(result);
        }

        public async Task<ActionResult> ViewAttachements(string orderId)
        {
            List<AttachmentItem> attachments = new List<AttachmentItem>();
            ViewBag.orderId = orderId;
            if (string.IsNullOrEmpty(orderId))
                return View();

            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            if (order != null)
            {
                attachments = order.Attachments.OrderByDescending(a => a.Created).Select(a => new AttachmentItem
                {
                    Id = a.FileInfoId,
                    Url = "/Attachment/" + a.FileInfoId + "/preview"
                }).ToList();
            }

            return View(attachments);
        }

        public async Task<ActionResult> Process(string orderId)
        {
            var manager = new OrderOperationLogManager(User);
            var logs = await manager.GetOrderOperationLogs(orderId);
            ViewBag.orderId = orderId;
            return View(logs);
        }

        public async Task<JsonResult> Calendar(DateTime start, DateTime end)
        {
            var manager = new OrderManager(User);
            var calendars = await manager.GetOrderCalendarsAsync(start, end);
            return Json(true, string.Empty, calendars.Select(c => new
            {
                title = c.ProcessCount,
                start = c.Date.ToString(SaleManagentConstants.UI.DateStringFormat)
            }));
        }

        private static MemoryStream MakeThumbnail(Stream originalImageStream, int dHeight, int dWidth)
        {
            Image iSource = Image.FromStream(originalImageStream); ;//从指定的文件创建Image
            ImageFormat tFormat = iSource.RawFormat;//指定文件的格式并获取
            int sW = 0, sH = 0;//记录宽度和高度
            Size temsize = new Size(iSource.Width, iSource.Height);//实例化size。知矩形的高度和宽度
            if (temsize.Height > dHeight || temsize.Width > dWidth)//判断原图大小是否大于指定大小
            {
                if ((temsize.Width * dHeight) > (temsize.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * temsize.Height) / temsize.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (temsize.Width * dHeight) / temsize.Height;
                }
            }
            else//如果原图大小小于指定的大小
            {
                sW = temsize.Width;//原图宽度等于指定宽度
                sH = temsize.Height;//原图高度等于指定高度
            }
            Bitmap oB = new Bitmap(dWidth, dHeight);//实例化
            Graphics g = Graphics.FromImage(oB);//从指定的Image中创建Graphics
            g.Clear(Color.White);//设置画布背景颜色
            g.CompositingQuality = CompositingQuality.HighQuality;//合成图像的呈现质量
            g.SmoothingMode = SmoothingMode.HighQuality;//呈现质量
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;//插补模式
                                                                       //开始重新绘制图像
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();//释放资源
                        //保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();//用于向图像编码器传递值
            long[] qy = new long[1];
            qy[0] = 100;
            EncoderParameter eParm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParm;
            try
            {
                //获得包含有关内置图像编码器的信息的ImageCodeInfo对象
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageDecoders();
                ImageCodecInfo jpegICIinfo = null;
                MemoryStream thumbnailStream = new MemoryStream();
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    oB.Save(thumbnailStream, jpegICIinfo, ep);
                }
                else
                {
                    oB.Save(thumbnailStream, tFormat);// 已指定格式保存到指定文件
                }
                return thumbnailStream;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                iSource.Dispose();//释放资源
                oB.Dispose();
            }
        }

        private static byte[] GetByteImage(Image img)

        {

            byte[] bt = null;

            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);

                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Jpeg);//将图像以指定的格式存入缓存内存流

                    bt = new byte[mostream.Length];

                    mostream.Position = 0;//设置留的初始位置

                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));

                }

            }

            return bt;
        }

        private void SetOrderDeliveryDate(Order order, int shippingScheduleDays)
        {
            if (order.OrderRushStatus != OrderRushStatus.Normal)
            {
                order.DeliveryDate = order.Created.Date.AddDays(GetRushDays(order.OrderRushStatus));
                return;
            }

            if (!order.DeliveryDate.HasValue)
            {
                order.DeliveryDate = order.Created.Date.AddDays(shippingScheduleDays);
            }
        }

        private int GetRushDays(OrderRushStatus orderRushStatus)
        {
            switch (orderRushStatus)
            {
                case OrderRushStatus.Rush:
                    return SaleManagentConstants.UI.OrderRushDays;
                case OrderRushStatus.VeryRush:
                    return SaleManagentConstants.UI.OrderVeryRushDays;
                default:
                    return 0;
            }
        }
    }
}