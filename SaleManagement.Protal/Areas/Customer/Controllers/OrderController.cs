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
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Areas.Customer.Controllers
{
    [RoutePrefix("Order")]
    public class OrderController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(int start, int take, CustomerQueryOrderStatus status = CustomerQueryOrderStatus.All)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new OrderManager(User);
            Func<IQueryable<Order>, IQueryable<Order>> filter = query =>
            {
                query = query.Where(j => j.CustomerId == User.Id);

                if (status == CustomerQueryOrderStatus.Process)
                {
                    query = query.Where(j => j.OrderStatus != OrderStatus.UnConfirmed && j.OrderStatus != OrderStatus.Shipment && j.OrderStatus != OrderStatus.HaveGoods);
                }

                if (status == CustomerQueryOrderStatus.ForGoods)
                {
                    query = query.Where(j => j.OrderStatus == OrderStatus.Shipment);
                }

                if (status == CustomerQueryOrderStatus.HaveGoods)
                {
                    query = query.Where(j => j.OrderStatus == OrderStatus.HaveGoods);
                }

                if (status == CustomerQueryOrderStatus.CustomerTobeConfirm)
                {
                    query = query.Where(j => j.OrderStatus == OrderStatus.CustomerTobeConfirm);
                }
                return query;
            };

            var paging = await manager.GetOrdersAsync(start, take, filter);
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

        public async Task<ActionResult> Booking()
        {
            var model = new OrderViewModel();
            var manager = new BasicDataManager(User);
            model.ProductCategories = await manager.GetProductCategoriesAsync();
            model.ColorForms = await manager.GetColorFormsAsync();
            model.GemCategories = await manager.GetGemCategoriesAsync();
            return View(model);
        }

        public async Task<JsonResult> GoToCustomerConfirmStep(string orderId)
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

        private IList<AttachmentItem> GetAttachments(Order order)
        {
            var attachments = order.Attachments.OrderByDescending(a => a.Created).Take(2);
            return  attachments.Select( a =>
            {
                return new AttachmentItem
                {
                    Id = a.FileInfoId,
                    Url = "/Attachment/" + a.FileInfoId + "/Thumbnail"
                };
            }).ToList();
        }

        private async Task<InvokedResult> ChangeOrderStatus(string orderId, OrderStatus status)
        {
            var manager = new OrderManager(User);
            var order = await manager.GetOrderAsync(orderId);
            if (order == null)
                return InvokedResult.Fail("404", SaleManagentConstants.Errors.OrderNotFound);

            order.CurrentUserId = "";
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