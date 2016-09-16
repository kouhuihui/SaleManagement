using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
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
    }
}