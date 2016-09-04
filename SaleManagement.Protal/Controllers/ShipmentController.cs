using AutoMapper;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Shipment;
using SaleManagement.Protal.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class ShipmentController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(ShipmentOrdersQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new ShipmentManager(User);

            var paging = await manager.GetShipmentOrdersAsync(request.Start, request.Take, request.GetOrderListQueryFilter());
            var orders = paging.List.Select(u =>
            {
                var shipmentOrderViewModel = Mapper.Map<ShipmentOrder, ShipmentOrderViewModel>(u);
                shipmentOrderViewModel.Created = u.Created.ToString(SaleManagentConstants.UI.DateStringFormat);
                shipmentOrderViewModel.DeliveryDate = u.DeliveryDate.ToString(SaleManagentConstants.UI.DateStringFormat);
                shipmentOrderViewModel.AuditDate = u.AuditDate.HasValue ? u.AuditDate.Value.ToString(SaleManagentConstants.UI.DateStringFormat) : "";
                return shipmentOrderViewModel;
            });

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = orders,
            });
        }

        public async Task<ActionResult> Create([NamedModelBinder(typeof(CommaSeparatedModelBinder), "orderIds")] string[] orderIds)
        {
            if (!orderIds.Any())
                return Error("订单号不能为空");

            var orderManager = new OrderManager(User);
            var orders = await orderManager.GetOrdersAsync(orderIds);
            if (!orders.All(r => r.OrderStatus == OrderStatus.ToBeShip))
                return Error("订单号状态不是待出货");

            var customers = orders.Select(r => r.Customer).Distinct().ToList();
            if (customers.Count > 1)
                return Error("生成订单不能选择了多个公司");

            var customer = customers.FirstOrDefault();

            var discountRateManager = new DiscountRateManager();
            var discountRate = await discountRateManager.GetCustomerDiscountRateAsync(customer.Id);
            if (discountRate == null)
                discountRate = new CustomerDiscountRate();

            var shipmentOrderViewModel = new ShipmentOrderViewModel();

            shipmentOrderViewModel.ShipmentOrderInfos = await Task.WhenAll(orders.Select(async o =>
            {
                var dailyGoldPriceManager = new DailyGoldPriceManager();
                var dailyGoldPrice = await dailyGoldPriceManager.GetDailyGoldPriceAsync(o.ColorFormId, o.Created.Date);
                var shipmentOrderInfoViewModel = new ShipmentOrderInfoViewModel(o)
                {
                    GoldPrice = dailyGoldPrice == null ? 0 : dailyGoldPrice.Price,
                    LossRate = o.ColorForm.Name.ToLower().Contains("pt") ? discountRate.LossPt : discountRate.Loss18K
                };
                shipmentOrderInfoViewModel.TotalSetStoneWorkingCost = shipmentOrderInfoViewModel.OrderSetStoneInfos.Sum(r => r.SetStoneWorkingCost) * ((double)discountRate.StoneSetter / 100);
                shipmentOrderInfoViewModel.SideStoneNumber = shipmentOrderInfoViewModel.OrderSetStoneInfos.Sum(r => r.Number);
                shipmentOrderInfoViewModel.SideStoneWeight = shipmentOrderInfoViewModel.OrderSetStoneInfos.Sum(r => r.Weight);
                shipmentOrderInfoViewModel.SideStoneTotalAmount = shipmentOrderInfoViewModel.OrderSetStoneInfos.Sum(r => r.TotalAmount) * ((double)discountRate.SideStone / 100);
                return shipmentOrderInfoViewModel;
            }));
            shipmentOrderViewModel.CustomerName = customer.Name;
            shipmentOrderViewModel.CustomerId = customer.Id;
            shipmentOrderViewModel.TotalNumber = shipmentOrderViewModel.ShipmentOrderInfos.Sum(r => r.Number);
            return View(shipmentOrderViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Create(ShipmentOrderViewModel shipmentOrderViewModel)
        {
            var shipmentOrder = new ShipmentOrder();
            shipmentOrder = Mapper.Map<ShipmentOrderViewModel, ShipmentOrder>(shipmentOrderViewModel);

            var serialNumberManager = new SerialNumberManager(User);
            shipmentOrder.Id = SaleManagentConstants.Misc.ShipmentOrderPrefix + await serialNumberManager.NextSNAsync(SaleManagentConstants.SerialNames.ShipmentOrder);
            shipmentOrder.ShipmentOrderInfos.Each(r => r.ShipmentOrderId = shipmentOrder.Id);
            var shipmentManager = new ShipmentManager(User);
            var result = await shipmentManager.CreateAsync(shipmentOrder);
            if (result.Succeeded)
            {
                var orderIds = shipmentOrder.ShipmentOrderInfos.Select(r => r.Id);
                await new OrderManager(User).UpdateOrderStatusAsync(OrderStatus.Shipmenting, orderIds);
                await new OrderOperationLogManager(User).AddLogAsync(OrderStatus.Shipmenting, orderIds);
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Audit(string id)
        {
            var manager = new ShipmentManager(User);
            var shipmentOrder = await manager.GetShipmentOrderAsync(id);
            shipmentOrder.AuditStatus = ShipmentOrderAduitStatus.Pass;
            shipmentOrder.AuditorName = User.Name;
            shipmentOrder.AuditDate = DateTime.Now;
            var result = await manager.UpdateAsync(shipmentOrder);
            if (result.Succeeded)
            {
                var orderIds = shipmentOrder.ShipmentOrderInfos.Select(r => r.Id);
                await new OrderManager(User).UpdateOrderStatusAsync(OrderStatus.Shipment, orderIds);
                await new OrderOperationLogManager(User).AddLogAsync(OrderStatus.Shipment, orderIds);
                var reconciliation = new Reconciliation
                {
                    Amount = shipmentOrder.TotalAmount,
                    CompanyId = User.CompanyId,
                    Created = DateTime.Now,
                    CreatorId = User.Id,
                    CustomerId = shipmentOrder.CustomerId,
                    CustomerName = shipmentOrder.CustomerName,
                    Type = ReconciliationType.Arrearage
                };
                await new ReconciliationManager(User).CreateAsync(reconciliation);
            }
            return Json(result);
        }

        public async Task<FileStreamResult> Export(ShipmentOrdersQueryRequest request)
        {
            var manager = new ShipmentManager(User);
            var shipmentOrders = await manager.GetShipmentOrders(request.GetOrderListQueryFilter());
            var titles = new string[] { "序号", "客户", "订单号", "品类", "出货日期", "件数", "净金重(g)", "含耗重(g)", "金料额", "副石数", "副石重", "镶石工费", "副石额", "基本工费", "起版/出蜡", "石值/风险", "其他工艺", "总额" };
            var result = Dickson.Web.Helper.ExcelHelp.Export(titles, "出货单明细", ws =>
            {
                var row = 2;
                int index = 1;
                foreach (var shipmentOrder in shipmentOrders)
                {
                    foreach (var shipmentOrderInfo in shipmentOrder.ShipmentOrderInfos)
                    {
                        ws.Cells[row, 1].Value = index;
                        ws.Cells[row, 2].Value = shipmentOrder.CustomerName;
                        ws.Cells[row, 3].Value = shipmentOrder.Id;
                        ws.Cells[row, 4].Value = shipmentOrderInfo.Order.ProductCategory.Name;
                        ws.Cells[row, 5].Value = shipmentOrder.DeliveryDate.ToString(SaleManagentConstants.UI.DateStringFormat);
                        ws.Cells[row, 6].Value = shipmentOrderInfo.Order.Number;
                        ws.Cells[row, 7].Value = shipmentOrderInfo.GoldWeight;
                        ws.Cells[row, 8].Value = shipmentOrderInfo.GoldWeight * (1 + shipmentOrderInfo.LossRate / 100);
                        ws.Cells[row, 9].Value = shipmentOrderInfo.GoldPrice;
                        ws.Cells[row, 10].Value = shipmentOrderInfo.SideStoneNumber;
                        ws.Cells[row, 11].Value = shipmentOrderInfo.SideStoneWeight;
                        ws.Cells[row, 12].Value = shipmentOrderInfo.TotalSetStoneWorkingCost;
                        ws.Cells[row, 13].Value = shipmentOrderInfo.SideStoneTotalAmount;
                        ws.Cells[row, 14].Value = shipmentOrderInfo.BasicCost;
                        ws.Cells[row, 15].Value = shipmentOrderInfo.OutputWaxCost;
                        ws.Cells[row, 16].Value = shipmentOrderInfo.RiskFee;
                        ws.Cells[row, 17].Value = shipmentOrderInfo.OtherCost;
                        ws.Cells[row, 18].Value = shipmentOrderInfo.TotalAmount;
                        row++;
                        index++;
                    }
                };
            });
            return result;
        }
    }
}