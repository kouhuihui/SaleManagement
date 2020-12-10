using AutoMapper;
using Dickson.Core.ComponentModel;
using Dickson.Web.Extensions;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Areas.Customer.Models.Order;
using SaleManagement.Protal.Models.Order;
using SaleManagement.Protal.Models.Shipment;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Areas.Customer.Controllers
{
    [RoutePrefix("Order")]
    public class OrderController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(CustomerOrdersQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View(request);

            var manager = new OrderManager(User);
            Func<IQueryable<Order>, IQueryable<Order>> filter = query =>
            {
                query = query.Where(j => j.CustomerId == User.Id);

                if (!string.IsNullOrEmpty(request.OrderId))
                {
                    query = query.Where(j => j.Id.Contains(request.OrderId));
                }

                if (request.QueryOrderStatus == CustomerQueryOrderStatus.Process)
                {
                    query = query.Where(j => j.OrderStatus != OrderStatus.Delete && j.OrderStatus != OrderStatus.UnConfirmed && j.OrderStatus != OrderStatus.Shipment && j.OrderStatus != OrderStatus.HaveGoods);
                }

                if (request.QueryOrderStatus == CustomerQueryOrderStatus.ForGoods)
                {
                    query = query.Where(j => j.OrderStatus == OrderStatus.Shipment);
                }

                if (request.QueryOrderStatus == CustomerQueryOrderStatus.HaveGoods)
                {
                    query = query.Where(j => j.OrderStatus == OrderStatus.HaveGoods);
                }

                if (request.QueryOrderStatus == CustomerQueryOrderStatus.CustomerTobeConfirm)
                {
                    query = query.Where(j => j.OrderStatus == OrderStatus.CustomerTobeConfirm);
                }

                if (request.QueryOrderStatus == CustomerQueryOrderStatus.WaitStone)
                {
                    query = query.Where(j => j.OrderStatus == OrderStatus.WaitStone);
                }
                return query;
            };

            var paging = await manager.GetOrdersAsync(request.Start, request.Take, filter);
            var orders = paging.List.Select(u => new OrderListItemViewModel(u)
            {
                Attachments = GetAttachments(u)
            });

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = orders,
            });
        }


        [PagingParameterInspector]
        public async Task<ActionResult> MyOrders(OrderQueryRequestBase request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new OrderManager(User);
            Func<IQueryable<Order>, IQueryable<Order>> filter = query =>
            {
                query = query.Where(j => j.CustomerId == User.Id);

                if (!string.IsNullOrEmpty(request.OrderId))
                {
                    var orderIds = request.OrderId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    query = query.Where(f => orderIds.Any(o => f.Id.Contains(o)));
                }

                if (request.ColorFormId.HasValue)
                {
                    query = query.Where(f => f.ColorFormId == request.ColorFormId.Value);
                }

                if (request.Status.HasValue)
                {
                    query = query.Where(f => f.OrderStatus == request.Status);
                }
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(f => f.Remark.Contains(request.Keyword));
                }
                return query;
            };

            var paging = await manager.GetOrdersAsync(request.Start, request.Take, filter);
            var orders = paging.List.Select(u => new OrderListItemViewModel(u));

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = orders,
            });
        }

        public async Task<ActionResult> Booking(string versionNo = "")
        {
            var model = new OrderViewModel();
            var manager = new BasicDataManager(User);
            model.ProductCategories = await manager.GetProductCategoriesAsync();
            model.ColorForms = await manager.GetColorFormsAsync();
            model.GemCategories = await manager.GetGemCategoriesAsync();
            if (!string.IsNullOrEmpty(versionNo))
            {
                var hotSellingManager = new HotSellingManager();
                var hotSelling = await hotSellingManager.GetHotSellingByNoAsync(versionNo);

                model.GemCategoryId = hotSelling.GemCategory.Id;
                model.ProductCategoryId = hotSelling.ProductCategory.Id;
                model.VersionNo = hotSelling.VersionNo;
                model.Budget = (int)hotSelling.ReferencePrice;
                model.Attachments = hotSelling.Attachments.Where(t => t.FileType == 0).OrderByDescending(a => a.Created).Select(a => new AttachmentItem
                {
                    Id = a.FileInfoId,
                    Url = "/Attachment/" + a.FileInfoId + "/Thumbnail"
                }).ToList();


            }
            return View(model);
        }

        public async Task<JsonResult> GoToConfirmStep(string orderId)
        {
            var result = await ChangeOrderStatus(orderId, OrderStatus.CustomerConfirm);
            return Json(result);
        }

        public async Task<JsonResult> GoToHaveGoodsStep(string orderId)
        {
            var result = await ChangeOrderStatus(orderId, OrderStatus.HaveGoods);
            return Json(result);
        }

        public async Task<ActionResult> Process(string orderId)
        {
            var manager = new OrderOperationLogManager(User);
            var logs = await manager.GetOrderOperationLogs(orderId);
            ViewBag.orderId = orderId;
            return View(logs);
        }

        public async Task<ActionResult> ShipmentOrderDetail(string orderId)
        {
            var manager = new ShipmentManager(User);
            var shipmentOrder = await manager.GetShipmentOrderByOrderIdAsync(orderId);
            var isWeChat = OwinContext.GetBrowser().IsWeChat;
            if (!isWeChat)
                return RedirectToAction("Detail", "shipment", new { id = shipmentOrder.Id, Area = "" });

            var shipmentOrderViewModel = Mapper.Map<ShipmentOrder, ShipmentOrderViewModel>(shipmentOrder);
            return View(shipmentOrderViewModel);
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

        public async Task<ActionResult> ShipmentStatistics(ShipmentReportQuery reportQuery)
        {
            if (!Request.IsAjaxRequest())
                return View(reportQuery);

            reportQuery.CustomerId = User.Id;
            var shipmentOrderInfoViewModels = await GetShipmentOrderInfoViewModels(reportQuery);
            return Json(true, string.Empty, shipmentOrderInfoViewModels);
        }

        public async Task<FileStreamResult> ShipmentStatisticsExport(ShipmentReportQuery reportQuery)
        {
            reportQuery.CustomerId = User.Id;
            var shipmentOrderInfoViewModels = await GetShipmentOrderInfoViewModels(reportQuery);

            var titles = new string[] { "序号", "订单号", "品类", "出货日期", "件数", "总重(g)", "主石重", "净金重(g)", "含耗重(g)", "金料额", "副石数", "副石重", "镶石工费", "副石额", "基本工费", "出蜡倒模", "石值/风险", "其他工艺", "总额" };
            var result = Dickson.Web.Helper.ExcelHelp.Export(titles, "出货单明细", ws =>
            {
                var row = 2;
                int index = 1;

                foreach (var shipmentOrderInfo in shipmentOrderInfoViewModels)
                {
                    ws.Cells[row, 1].Value = index;
                    ws.Cells[row, 2].Value = shipmentOrderInfo.Id;
                    ws.Cells[row, 3].Value = shipmentOrderInfo.ProductCategoryName;
                    ws.Cells[row, 4].Value = shipmentOrderInfo.DeliveryDate;
                    ws.Cells[row, 5].Value = shipmentOrderInfo.Number;
                    ws.Cells[row, 6].Value = shipmentOrderInfo.Weight;
                    ws.Cells[row, 7].Value = shipmentOrderInfo.MainStoneSize;
                    ws.Cells[row, 8].Value = shipmentOrderInfo.GoldWeight;
                    ws.Cells[row, 9].Value = Math.Round(shipmentOrderInfo.GoldWeight * (1 + shipmentOrderInfo.LossRate / 100), 2);
                    ws.Cells[row, 10].Value = shipmentOrderInfo.GoldAmount;
                    ws.Cells[row, 11].Value = shipmentOrderInfo.SideStoneNumber;
                    ws.Cells[row, 12].Value = shipmentOrderInfo.SideStoneWeight;
                    ws.Cells[row, 13].Value = shipmentOrderInfo.TotalSetStoneWorkingCost;
                    ws.Cells[row, 14].Value = shipmentOrderInfo.SideStoneTotalAmount;
                    ws.Cells[row, 15].Value = shipmentOrderInfo.BasicCost;
                    ws.Cells[row, 16].Value = shipmentOrderInfo.OutputWaxCost;
                    ws.Cells[row, 17].Value = shipmentOrderInfo.RiskFee;
                    ws.Cells[row, 18].Value = shipmentOrderInfo.OtherCost;
                    ws.Cells[row, 19].Value = shipmentOrderInfo.TotalAmount;
                    row++;
                    index++;
                }
            });
            return result;
        }

        private async Task<IEnumerable<ShipmentOrderInfoViewModel>> GetShipmentOrderInfoViewModels(ShipmentReportQuery reportQuery)
        {
            var manager = new ShipmentManager(User);

            var shipmentOrderInfos = await manager.GetShipmentOrderInfosAsync(reportQuery.GetShipmentOrderInfosQueryFilter());
            var shipmentOrderInfoViewModels = shipmentOrderInfos.Select(f =>
            {
                var shipmentOrderInfoViewModel = Mapper.Map<ShipmentOrderInfo, ShipmentOrderInfoViewModel>(f);
                shipmentOrderInfoViewModel.Hhz = Math.Round(f.GoldWeight * (1 + f.LossRate / 100), 2);
                return shipmentOrderInfoViewModel;
            }).ToList();
            if (shipmentOrderInfoViewModels.Any())
            {
                var total = new ShipmentOrderInfoViewModel
                {
                    Id = "总计",
                    Number = shipmentOrderInfoViewModels.Sum(r => r.Number),
                    GoldWeight = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.GoldWeight), 2),
                    Hhz = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.Hhz), 2),
                    GoldAmount = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.GoldAmount), 2),
                    SideStoneNumber = shipmentOrderInfoViewModels.Sum(r => r.SideStoneNumber),
                    SideStoneWeight = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.SideStoneWeight), 2),
                    TotalSetStoneWorkingCost = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.TotalSetStoneWorkingCost), 2),
                    SideStoneTotalAmount = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.SideStoneTotalAmount), 2),
                    BasicCost = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.BasicCost), 2),
                    OutputWaxCost = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.OutputWaxCost), 2),
                    RiskFee = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.RiskFee), 2),
                    OtherCost = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.OtherCost), 2),
                    TotalAmount = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.TotalAmount), 2),
                    Weight = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.Weight), 2),
                    MainStoneSize = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.MainStoneSize), 2),
                };
                shipmentOrderInfoViewModels.Add(total);
            }
            return shipmentOrderInfoViewModels;
        }

        private IList<AttachmentItem> GetAttachments(Order order)
        {
            var attachments = order.Attachments.OrderByDescending(a => a.Created).Take(2);
            return attachments.Select(a =>
          {
              return new AttachmentItem
              {
                  Id = a.FileInfoId,
                  Url = "/Attachment/" + a.FileInfoId + "/Thumbnail"
              };
          }).ToList();
        }

        private async Task<InvokedResult> ChangeOrderStatus(string orderId, OrderStatus status, string currentUserId = "")
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            if (order == null)
                return InvokedResult.Fail("404", SaleManagentConstants.Errors.OrderNotFound);

            if (!string.IsNullOrEmpty(currentUserId))
            {
                order.CurrentUserId = currentUserId;
            }
            order.OrderStatus = status;
            var result = await manager.UpdateOrderAsync(order);
            if (result.Succeeded)
            {
                var operationLogManager = new OrderOperationLogManager(User);
                await operationLogManager.AddLogAsync(order.OrderStatus, order.Id);
            }
            return result;
        }
    }
}