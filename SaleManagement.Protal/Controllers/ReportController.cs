using AutoMapper;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Shipment;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class ReportController : PortalController
    {
        /// <summary>
        /// 账目统计
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AccountStatistics(ReportQueryBaseDto reportQuery)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new ReconciliationManager(User);
            var list = await manager.GetAccountStatisticsAsync(reportQuery);
            return Json(true, string.Empty, list);
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

            var titles = new string[] { "序号", "客户", "订单号", "品类", "出货日期", "件数", "净金重(g)", "含耗重(g)", "金料额", "副石数", "副石重", "镶石工费", "副石额", "基本工费", "出蜡倒模", "石值/风险", "其他工艺", "总额" };
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
                    ws.Cells[row, 7].Value = shipmentOrderInfo.GoldWeight;
                    ws.Cells[row, 8].Value = Math.Round(shipmentOrderInfo.GoldWeight * (1 + shipmentOrderInfo.LossRate / 100), 2);
                    ws.Cells[row, 9].Value = shipmentOrderInfo.GoldAmount;
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
                shipmentOrderInfoViewModel.CustomerName = f.Order.Customer.Name;
                shipmentOrderInfoViewModel.Hhz = Math.Round(f.GoldWeight * (1 + f.LossRate / 100), 2);
                return shipmentOrderInfoViewModel;
            }).ToList();
            if (shipmentOrderInfoViewModels.Any())
            {
                var total = new ShipmentOrderInfoViewModel
                {
                    Number = shipmentOrderInfoViewModels.Sum(r => r.Number),
                    GoldWeight = shipmentOrderInfoViewModels.Sum(r => r.GoldWeight),
                    Hhz = shipmentOrderInfoViewModels.Sum(r => r.Hhz),
                    GoldAmount = shipmentOrderInfoViewModels.Sum(r => r.GoldAmount),
                    SideStoneNumber = shipmentOrderInfoViewModels.Sum(r => r.SideStoneNumber),
                    SideStoneWeight = shipmentOrderInfoViewModels.Sum(r => r.SideStoneWeight),
                    TotalSetStoneWorkingCost = shipmentOrderInfoViewModels.Sum(r => r.TotalSetStoneWorkingCost),
                    SideStoneTotalAmount = shipmentOrderInfoViewModels.Sum(r => r.SideStoneTotalAmount),
                    BasicCost = shipmentOrderInfoViewModels.Sum(r => r.BasicCost),
                    OutputWaxCost = shipmentOrderInfoViewModels.Sum(r => r.OutputWaxCost),
                    RiskFee = shipmentOrderInfoViewModels.Sum(r => r.RiskFee),
                    OtherCost = shipmentOrderInfoViewModels.Sum(r => r.OtherCost),
                    TotalAmount = shipmentOrderInfoViewModels.Sum(r => r.TotalAmount),
                };
                shipmentOrderInfoViewModels.Add(total);
            }
            return shipmentOrderInfoViewModels;
        }
    }
}