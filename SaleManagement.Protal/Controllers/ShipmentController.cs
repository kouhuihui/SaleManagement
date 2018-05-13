using AutoMapper;
using Dickson.Core.ComponentModel;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Shipment;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class ShipmentController : PortalController
    {
        [UrlAuthorize]
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
                shipmentOrderViewModel.TotalAmount = Math.Round(shipmentOrderViewModel.TotalAmount, 2);
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
            //if (!orders.All(r => r.OrderStatus == OrderStatus.ToBeShip))
            //    return Error("订单号状态不是待出货");

            var customers = orders.Select(r => r.Customer).Distinct().ToList();
            if (customers.Count > 1)
                return Error("生成出货单不能选择了多个公司");

            var customer = customers.FirstOrDefault();

            var discountRateManager = new DiscountRateManager();
            var discountRate = await discountRateManager.GetCustomerDiscountRateAsync(customer.Id);

            var shipmentOrderViewModel = new ShipmentOrderViewModel()
            {
                SideStoneRate = (double)discountRate.SideStone / 100,
                StoneSetterRate = (double)discountRate.StoneSetter / 100
            };

            shipmentOrderViewModel.ShipmentOrderInfos = await Task.WhenAll(orders.Select(async o =>
                {
                    var dailyGoldPriceManager = new DailyGoldPriceManager();
                    var dailyGoldPrice = await dailyGoldPriceManager.GetNewDailyGoldPriceAsync(o.ColorFormId);
                    var shipmentOrderInfoViewModel = new ShipmentOrderInfoViewModel(o)
                    {
                        GoldPrice = dailyGoldPrice == null ? 0 : dailyGoldPrice.Price,
                        LossRate = o.ColorForm.Name.ToLower().Contains("pt") ? discountRate.LossPt : discountRate.Loss18K,
                    };

                    shipmentOrderInfoViewModel.Hhz = Math.Round(shipmentOrderInfoViewModel.GoldWeight * (1 + shipmentOrderInfoViewModel.LossRate / 100), 2);
                    shipmentOrderInfoViewModel.GoldAmount = shipmentOrderInfoViewModel.GoldPrice * shipmentOrderInfoViewModel.Hhz;
                    shipmentOrderInfoViewModel.TotalSetStoneWorkingCost = shipmentOrderInfoViewModel.OrderSetStoneInfos.Sum(r => r.SetStoneWorkingCost) * ((double)discountRate.StoneSetter / 100);
                    shipmentOrderInfoViewModel.SideStoneNumber = shipmentOrderInfoViewModel.OrderSetStoneInfos.Sum(r => r.Number);
                    shipmentOrderInfoViewModel.SideStoneWeight = shipmentOrderInfoViewModel.OrderSetStoneInfos.Sum(r => r.Weight);
                    shipmentOrderInfoViewModel.SideStoneTotalAmount = shipmentOrderInfoViewModel.OrderSetStoneInfos.Sum(r => r.TotalAmount) * ((double)discountRate.SideStone / 100); ;
                    shipmentOrderInfoViewModel.RushCost = GetOrderRushCost(o);
                    shipmentOrderInfoViewModel.RiskFee = o.IsInsure ? GetRiskFee(o.Insurance, o.RiskType) : 0;
                    return shipmentOrderInfoViewModel;
                }));
            shipmentOrderViewModel.CustomerName = customer.Name;
            shipmentOrderViewModel.CustomerId = customer.Id;
            shipmentOrderViewModel.TotalNumber = shipmentOrderViewModel.ShipmentOrderInfos.Sum(r => r.Number);

            return View(shipmentOrderViewModel);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var manager = new ShipmentManager(User);
            var shipmentOrder = await manager.GetShipmentOrderAsync(id);
            var shipmentOrderViewModel = Mapper.Map<ShipmentOrder, ShipmentOrderViewModel>(shipmentOrder);

            var discountRateManager = new DiscountRateManager();
            var discountRate = await discountRateManager.GetCustomerDiscountRateAsync(shipmentOrder.CustomerId);

            shipmentOrderViewModel.SideStoneRate = (double)discountRate.SideStone / 100;
            shipmentOrderViewModel.ShipmentOrderInfos.Each(f =>
            {
                f.Hhz = Math.Round(f.GoldWeight * (1 + f.LossRate / 100), 2);
                f.GoldAmount = f.GoldPrice * f.Hhz;
                f.TotalSetStoneWorkingCost = f.OrderSetStoneInfos.Sum(r => r.SetStoneWorkingCost) * ((double)discountRate.StoneSetter / 100);
                f.SideStoneNumber = f.OrderSetStoneInfos.Sum(r => r.Number);
                f.SideStoneTotalAmount = f.OrderSetStoneInfos.Sum(r => r.TotalAmount) * ((double)discountRate.SideStone / 100);
                f.SideStoneWeight = f.OrderSetStoneInfos.Sum(r => r.Weight);
            }
            );
            return View("create", shipmentOrderViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Create(ShipmentOrderViewModel shipmentOrderViewModel)
        {
            var shipmentOrder = new ShipmentOrder();
            shipmentOrder = Mapper.Map<ShipmentOrderViewModel, ShipmentOrder>(shipmentOrderViewModel);

            var usermanager = new UserManager();
            var customer = usermanager.FindByIdByCache(shipmentOrder.CustomerId);
            shipmentOrder.CustomerName = customer.Name;

            var serialNumberManager = new SerialNumberManager(User);
            shipmentOrder.Id = SaleManagentConstants.Misc.ShipmentOrderPrefix + await serialNumberManager.NextSNAsync(SaleManagentConstants.SerialNames.ShipmentOrder);
            shipmentOrder.CompanyId = User.CompanyId;
            shipmentOrder.ShipmentOrderInfos.Each(r => r.ShipmentOrderId = shipmentOrder.Id);
            var shipmentManager = new ShipmentManager(User);
            var result = await shipmentManager.CreateAsync(shipmentOrder);
            if (result.Succeeded)
            {
                var orderIds = shipmentOrder.ShipmentOrderInfos.Select(r => r.Id);
                await new OrderManager(User).UpdateOrderStatusAsync(OrderStatus.Shipmenting, orderIds);
                await new OrderOperationLogManager(User).AddLogAsync(OrderStatus.Shipmenting, orderIds);
            }
            return Json(result.Succeeded, "", result.Data);
        }

        [HttpPost]
        public async Task<JsonResult> Edit(ShipmentOrderViewModel shipmentOrderViewModel)
        {
            var shipmentOrder = new ShipmentOrder();

            var shipmentManager = new ShipmentManager(User);
            shipmentOrder = await shipmentManager.GetShipmentOrderAsync(shipmentOrderViewModel.Id);
            shipmentOrder = Mapper.Map<ShipmentOrderViewModel, ShipmentOrder>(shipmentOrderViewModel);
            shipmentOrder.ShipmentOrderInfos.Each(r => r.ShipmentOrderId = shipmentOrder.Id);

            var result = await shipmentManager.UpdateAsync(shipmentOrder);
            return Json(result.Succeeded, "", result.Data);
        }

        [HttpPost]
        public async Task<JsonResult> Audit(string id)
        {
            var manager = new ShipmentManager(User);
            var shipmentOrder = await manager.GetShipmentOrderAsync(id);
            if (shipmentOrder.AuditStatus == ShipmentOrderAduitStatus.Pass)
                return Json(false, "出货单已经通过审核，不能再次审核");

            shipmentOrder.AuditStatus = ShipmentOrderAduitStatus.Pass;
            shipmentOrder.AuditorName = User.Name;
            shipmentOrder.AuditDate = DateTime.Now;
            var result = await manager.AuditShipmentOrder(shipmentOrder);
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
                    Type = ReconciliationType.Arrearage,
                    Remark = $"{id}出货"
                };
                await new ReconciliationManager(User).CreateAsync(reconciliation);
                await SendShipmentOrderMsg(shipmentOrder);
            }
            return Json(result);
        }

        private async Task<JsonResult> SendShipmentOrderMsg(ShipmentOrder shipmentOrder)
        {
            if (shipmentOrder.ShipmentOrderInfos.Any())
            {

                var accountBindingManager = new AccountBindingManager();
                var accountbing = await accountBindingManager.GetAccountBindingByCustomerId(shipmentOrder.CustomerId);

                if (accountbing == null)
                    return Json(InvokedResult.Fail("404", "客户未绑定微信"));

                SendMesHelp.SendNews(new NewsMes()
                {
                    OpenId = accountbing.WxAccount,
                    Articles = new List<Article>()
                    {
                        new Article()
                        {
                            Title = "订单已发货",
                            Description =
                                string.Format("订单{0}已于{1}出货！",
                                    string.Join(",", shipmentOrder.ShipmentOrderInfos.Select(r => r.Id)),
                                    DateTime.Now.ToShortDateString()),
                            Url = "https://www.18k.hk/customer/order/list?status=2",
                            PicUrl = "https://www.18k.hk/static/images/ddzs.jpg"
                        }
                    }
                });
            }
            return Json(InvokedResult.SucceededResult);
        }

        [HttpPost]
        public async Task<JsonResult> CreatedRecord([NamedModelBinder(typeof(CommaSeparatedModelBinder), "orderIds")] string[] orderIds)
        {
            var manager = new ShipmentManager(User);
            foreach (var id in orderIds)
            {
                var shipmentOrder = await manager.GetShipmentOrderAsync(id);
                if (shipmentOrder.AuditStatus == ShipmentOrderAduitStatus.Pass)
                    return Json(false, $"{id}出货单已经通过审核，不能再次审核");

                shipmentOrder.AuditStatus = ShipmentOrderAduitStatus.Pass;
                shipmentOrder.AuditorName = User.Name;
                shipmentOrder.AuditDate = DateTime.Now;
                var result = await manager.AuditShipmentOrder(shipmentOrder);
                if (result.Succeeded)
                {
                    var orderInfoId = shipmentOrder.ShipmentOrderInfos.Select(r => r.Id);
                    await new OrderManager(User).UpdateOrderStatusAsync(OrderStatus.Shipment, orderInfoId);
                }
            }
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> CancelAudit(string id)
        {
            var manager = new ShipmentManager(User);
            var shipmentOrder = await manager.GetShipmentOrderAsync(id);
            if (shipmentOrder == null)
                return Json(false, "出货单不存在");

            if (shipmentOrder.AuditStatus != ShipmentOrderAduitStatus.Pass)
                return Json(false, "出货单不是已审核状态");

            shipmentOrder.AuditStatus = ShipmentOrderAduitStatus.New;
            shipmentOrder.AuditorName = User.Name;
            shipmentOrder.AuditDate = DateTime.Now;
            var result = await manager.AuditShipmentOrder(shipmentOrder);
            if (result.Succeeded)
            {
                var orderIds = shipmentOrder.ShipmentOrderInfos.Select(r => r.Id);
                await new OrderManager(User).UpdateOrderStatusAsync(OrderStatus.Shipmenting, orderIds);
                await new OrderOperationLogManager(User).AddLogAsync(OrderStatus.Shipmenting, orderIds);

                await new ReconciliationManager(User).DeleteReconciliationAsync(shipmentOrder.Id);
            }
            return Json(result);
        }

        public async Task<ActionResult> Detail(string id)
        {
            var manager = new ShipmentManager(User);
            var shipmentOrder = await manager.GetShipmentOrderAsync(id);


            var discountRateManager = new DiscountRateManager();
            var discountRate = await discountRateManager.GetCustomerDiscountRateAsync(shipmentOrder.CustomerId);

            var shipmentOrderViewModel = Mapper.Map<ShipmentOrder, ShipmentOrderViewModel>(shipmentOrder);
            shipmentOrderViewModel.SideStoneRate = (double)discountRate.SideStone / 100;
            shipmentOrderViewModel.StoneSetterRate = (double)discountRate.StoneSetter / 100;

            shipmentOrderViewModel.ShipmentOrderInfos.Each(f => f.Hhz = Math.Round(f.GoldWeight * (1 + f.LossRate / 100), 2));

            shipmentOrderViewModel.TotalArrearage =
                await new ReconciliationManager(User).GetTotalSurplusArrearageAsync(shipmentOrder.CustomerId);
            return View(shipmentOrderViewModel);
        }

        public ActionResult CreateRepair()
        {
            var shipmentOrderViewModel = new ShipmentOrderViewModel();
            return View("Create", shipmentOrderViewModel);
        }


        private double GetOrderRushCost(Core.Models.Order order)
        {
            if (order.OrderRushStatus == Core.Models.OrderRushStatus.Normal)
                return 0;

            var now = DateTime.Now;
            if (order.OrderRushStatus == Core.Models.OrderRushStatus.VeryRush && now < order.DeliveryDate)
                return 300;

            if (now < order.Created.AddDays(8))
                return 100;

            return 0;
        }

        private double GetRiskFee(int Insurance, RiskType type)
        {
            double insuranceFee = 0;
            switch (type)
            {
                case RiskType.Low:
                    insuranceFee = Insurance > 3000 ? Insurance * 0.005 : 0;
                    break;
                case RiskType.Middle:
                    insuranceFee = Insurance * 0.01;
                    break;
                case RiskType.High:
                    insuranceFee = Insurance * 0.03;
                    break;
            }
            return insuranceFee;
        }
    }
}