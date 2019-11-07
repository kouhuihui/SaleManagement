using AutoMapper;
using Microsoft.Extensions.Logging;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Shipment;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class ReportController : PortalController
    {
        /// <summary>
        /// 账目统计
        /// </summary>
        /// <returns></returns>
        [UrlAuthorize]
        public async Task<ActionResult> AccountStatistics(ReportQueryBaseDto reportQuery)
        {
            if (!Request.IsAjaxRequest())
                return View(reportQuery);

            var manager = new ReconciliationManager(User);
            var list = await manager.GetAccountStatisticsAsync(reportQuery);
            return Json(true, string.Empty, list);
        }

        public async Task<JsonResult> GetTotalSurplusArrearage(string customerId)
        {
            var manager = new ReconciliationManager(User);
            var result = await manager.GetTotalSurplusArrearageAsync(customerId);
            return Json(true, string.Empty, result);
        }

        public async Task<FileStreamResult> AccountStatisticsExport(ReportQueryBaseDto reportQuery)
        {
            var manager = new ReconciliationManager(User);
            var accountStatistics = await manager.GetAccountStatisticsAsync(reportQuery);
            var titles = new string[] { "序号", "客户", "累积消费（元）", "当前欠款（元）" };
            var result = Dickson.Web.Helper.ExcelHelp.Export(titles, "账目报表", ws =>
            {
                var row = 2;
                int index = 1;
                foreach (var accountStatistic in accountStatistics)
                {

                    ws.Cells[row, 1].Value = index;
                    ws.Cells[row, 2].Value = accountStatistic.CustomerName;
                    ws.Cells[row, 3].Value = accountStatistic.PaymentInQuery;
                    ws.Cells[row, 4].Value = accountStatistic.SurplusArrearage;
                    row++;
                    index++;
                };
            });
            return result;
        }

        [UrlAuthorize]
        public async Task<ActionResult> OrderSetStoneStatistics(SetStoneReportQuery reportQuery)
        {
            if (!Request.IsAjaxRequest())
                return View(reportQuery);

            var manager = new ShipmentManager(User);
            var list = await manager.GetOrderSetStoneStatisticsAsync(reportQuery);
            return Json(true, string.Empty, list);
        }

        public async Task<FileStreamResult> OrderSetStoneStatisticsExport(SetStoneReportQuery reportQuery)
        {
            var manager = new ShipmentManager(User);
            var orderSetStoneStatistics = await manager.GetOrderSetStoneStatisticsAsync(reportQuery);
            var titles = new string[] { "序号", "配石名称", "重量（ct）", "数量", "副石额" };
            var result = Dickson.Web.Helper.ExcelHelp.Export(titles, "配石报表", ws =>
            {
                var row = 2;
                int index = 1;
                foreach (var orderSetStoneStatistic in orderSetStoneStatistics)
                {

                    ws.Cells[row, 1].Value = index;
                    ws.Cells[row, 2].Value = orderSetStoneStatistic.SetStoneName;
                    ws.Cells[row, 3].Value = orderSetStoneStatistic.Weight;
                    ws.Cells[row, 4].Value = orderSetStoneStatistic.Number;
                    ws.Cells[row, 5].Value = orderSetStoneStatistic.SetStoneAmount;
                    row++;
                    index++;
                };
            });
            return result;
        }

        [UrlAuthorize]
        public async Task<ActionResult> ShipmentStatistics(ShipmentReportQuery reportQuery)
        {
            if (!Request.IsAjaxRequest())
                return View(reportQuery);

            var shipmentOrderInfoViewModels = await GetShipmentOrderInfoViewModels(reportQuery);
            return Json(true, string.Empty, shipmentOrderInfoViewModels);
        }

        public async Task<FileStreamResult> ShipmentStatisticsExport(ShipmentReportQuery reportQuery)
        {
            var shipmentOrderInfoViewModels = await GetShipmentOrderInfoViewModels(reportQuery);

            var titles = new string[] { "序号", "客户", "订单号", "品类", "出货日期", "件数", "总重(g)", "主石重", "净金重(g)", "含耗重(g)", "金料额", "副石数", "副石重", "镶石工费", "副石额", "基本工费", "出蜡倒模", "石值/风险", "其他工艺", "总额", "正常/逾期" };
            var result = Dickson.Web.Helper.ExcelHelp.Export(titles, "出货单明细", ws =>
            {
                var row = 2;
                int index = 1;

                foreach (var shipmentOrderInfo in shipmentOrderInfoViewModels)
                {
                    ws.Cells[row, 1].Value = index;
                    ws.Cells[row, 2].Value = string.IsNullOrEmpty(shipmentOrderInfo.CustomerName) ? "总计：" : shipmentOrderInfo.CustomerName;
                    ws.Cells[row, 3].Value = shipmentOrderInfo.Id;
                    ws.Cells[row, 4].Value = shipmentOrderInfo.ProductCategoryName;
                    ws.Cells[row, 5].Value = shipmentOrderInfo.DeliveryDate;
                    ws.Cells[row, 6].Value = shipmentOrderInfo.Number;
                    ws.Cells[row, 7].Value = shipmentOrderInfo.Weight;
                    ws.Cells[row, 8].Value = shipmentOrderInfo.MainStoneSize;
                    ws.Cells[row, 9].Value = shipmentOrderInfo.GoldWeight;
                    ws.Cells[row, 10].Value = shipmentOrderInfo.Hhz;
                    ws.Cells[row, 11].Value = shipmentOrderInfo.GoldAmount;
                    ws.Cells[row, 12].Value = shipmentOrderInfo.SideStoneNumber;
                    ws.Cells[row, 13].Value = shipmentOrderInfo.SideStoneWeight;
                    ws.Cells[row, 14].Value = shipmentOrderInfo.TotalSetStoneWorkingCost;
                    ws.Cells[row, 15].Value = shipmentOrderInfo.SideStoneTotalAmount;
                    ws.Cells[row, 16].Value = shipmentOrderInfo.BasicCost;
                    ws.Cells[row, 17].Value = shipmentOrderInfo.OutputWaxCost;
                    ws.Cells[row, 18].Value = shipmentOrderInfo.RiskFee;
                    ws.Cells[row, 19].Value = shipmentOrderInfo.OtherCost;
                    ws.Cells[row, 20].Value = shipmentOrderInfo.TotalAmount;
                    ws.Cells[row, 21].Value = shipmentOrderInfo.IsShipOnTime;
                    row++;
                    index++;
                }
            });
            return result;
        }

        private async Task<IEnumerable<ShipmentOrderInfoViewModel>> GetShipmentOrderInfoViewModels(ShipmentReportQuery reportQuery)
        {
            var manager = new ShipmentManager(User);

            var stopwatch = Stopwatch.StartNew();
            var shipmentOrderInfos = await manager.GetShipmentOrderInfosAsync(reportQuery.GetShipmentOrderInfosQueryFilter());
            stopwatch.Stop();
            var shipmentOrderInfoViewModels = shipmentOrderInfos.Select(f =>
            {
                var shipmentOrderInfoViewModel = Mapper.Map<ShipmentOrderInfo, ShipmentOrderInfoViewModel>(f);
                shipmentOrderInfoViewModel.CustomerName = f.Order.Customer.Name;
                shipmentOrderInfoViewModel.Hhz = Math.Round(f.GoldWeight * (1 + f.LossRate / 100), 2);
                shipmentOrderInfoViewModel.IsShipOnTime = f.Order.DeliveryDate.HasValue && f.ShipmentOrder.DeliveryDate < f.Order.DeliveryDate.Value ? "正常" : "逾期";
                shipmentOrderInfoViewModel.DeliveryDate = f.ShipmentOrder.DeliveryDate.ToShortDateString();
                return shipmentOrderInfoViewModel;
            }).ToList();

            var stopwatch2 = Stopwatch.StartNew();
            if (shipmentOrderInfoViewModels.Any())
            {
                var total = new ShipmentOrderInfoViewModel
                {
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
                    IsShipOnTime = $"正常{shipmentOrderInfoViewModels.Count(r => r.IsShipOnTime == "正常")},逾期{shipmentOrderInfoViewModels.Count(r => r.IsShipOnTime == "逾期")}",
                    MainStoneSize = Math.Round(shipmentOrderInfoViewModels.Sum(r => r.MainStoneSize), 2)
                };
                shipmentOrderInfoViewModels.Add(total);
            }
            stopwatch2.Stop();
            LoggerHelper.Logger.LogInformation($"获取数据耗时{stopwatch.ElapsedMilliseconds}毫秒,统计数据耗时{stopwatch2.ElapsedMilliseconds}毫秒");
            return shipmentOrderInfoViewModels;
        }


        [UrlAuthorize]
        public async Task<ActionResult> OrderMainStoneStatistics(ReportQueryBaseDto reportQuery)
        {
            if (!Request.IsAjaxRequest())
                return View(reportQuery);

            var manager = new OrderMainStoneInfoManager(User);
            var list = await manager.GetOrderMainStoneStatisticsAsync(reportQuery);
            return Json(true, string.Empty, list);
        }

        [UrlAuthorize]
        public async Task<ActionResult> DesginCostStatistics(ReportQueryBaseDto reportQuery)
        {
            if (!Request.IsAjaxRequest())
                return View(reportQuery);

            var list = await GetDesginCostStatistic(reportQuery);

            return Json(true, string.Empty, list);
        }

        private async Task<IEnumerable<DesginCostStatistic>> GetDesginCostStatistic(ReportQueryBaseDto reportQuery)
        {
            var manager = new OrderManager(User);
            var list = (await manager.DesginCostStatistics(reportQuery)).ToList();

            if (list.Any())
            {
                var total = new DesginCostStatistic()
                {
                    DesginCost = list.Sum(r => r.DesginCost),
                    OutputWaxCost = list.Sum(r => r.OutputWaxCost)
                };
                list.Add(total);
            }

            return list;
        }

        public async Task<ActionResult> DesginCostStatisticsExport(ReportQueryBaseDto reportQuery)
        {
            var list = (await GetDesginCostStatistic(reportQuery)).ToList();

            var titles = new string[] { "序号", "订单", "支付设计成本（元）", "设计费（元）" };
            var result = Dickson.Web.Helper.ExcelHelp.Export(titles, "设计费用统计", ws =>
            {
                var row = 2;
                int index = 1;
                foreach (var desginCostStatistic in list)
                {

                    ws.Cells[row, 1].Value = index;
                    ws.Cells[row, 2].Value = string.IsNullOrEmpty(desginCostStatistic.Id)
                        ? "总计："
                        : desginCostStatistic.Id;
                    ws.Cells[row, 3].Value = desginCostStatistic.DesginCost;
                    ws.Cells[row, 4].Value = desginCostStatistic.OutputWaxCost;
                    row++;
                    index++;
                };
            });
            return result;
        }



        public async Task<FileStreamResult> OrderMainStoneStatisticsExport(ShipmentReportQuery reportQuery)
        {

            var manager = new OrderMainStoneInfoManager(User);
            var mainStones = await manager.GetOrderMainStoneStatisticsAsync(reportQuery);

            var titles = new string[] { "序号", "客户", "单号", "主石名称", "主石大小", "风险等级", "收石日期" };
            var result = Dickson.Web.Helper.ExcelHelp.Export(titles, "主石收石记录", ws =>
            {
                var row = 2;
                int index = 1;

                foreach (var mainStone in mainStones)
                {
                    ws.Cells[row, 1].Value = index;
                    ws.Cells[row, 2].Value = mainStone.CustomerName;
                    ws.Cells[row, 3].Value = mainStone.OrderId;
                    ws.Cells[row, 4].Value = mainStone.MainStoneName;
                    ws.Cells[row, 5].Value = mainStone.MainStoneWeight;
                    ws.Cells[row, 6].Value = mainStone.Risk;
                    ws.Cells[row, 7].Value = mainStone.Created;
                    row++;
                    index++;
                }
            });
            return result;
        }
    }
}