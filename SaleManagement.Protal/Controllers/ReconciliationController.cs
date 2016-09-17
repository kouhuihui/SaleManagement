using Dickson.Core.Common.Extensions;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Reconciliation;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class ReconciliationController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(ReconciliationQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View(request);

            var manager = new ReconciliationManager(User);

            var paging = await manager.GetReconciliationsAsync(request.Start, request.Take, request.GetReconciliationListQueryFilter());
            var reconciliations = paging.List.Select(u =>
            {
                return new ReconciliationItemViewModel(u);
            });

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = reconciliations,
            });
        }

        public ActionResult AddPayment()
        {
            var model = new ReconciliationItemViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddPayment(ReconciliationItemViewModel request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new ReconciliationManager(User);
            var reconciliation = new Reconciliation
            {
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                Amount = request.Amount,
                CompanyId = User.CompanyId,
                Type = Core.Models.ReconciliationType.Payment,
                Created = DateTime.Now,
                CreatorId = User.Id
            };
            var result = await manager.CreateAsync(reconciliation);
            return Json(true, string.Empty, result);
        }

        public async Task<FileStreamResult> Export(ReconciliationQueryRequest request)
        {
            var manager = new ReconciliationManager(User);
            var reconciliations = await manager.GetReconciliationsAsync(request.GetReconciliationListQueryFilter());
            var titles = new string[] { "序号", "客户","日期", "付/欠款", "金额(元)" };
            var result = Dickson.Web.Helper.ExcelHelp.Export(titles, "对账记录", ws =>
            {
                var row = 2;
                int index = 1;
                foreach (var reconciliation in reconciliations)
                {

                    ws.Cells[row, 1].Value = index;
                    ws.Cells[row, 2].Value = reconciliation.CustomerName;
                    ws.Cells[row, 3].Value = reconciliation.Created.ToString(SaleManagentConstants.UI.DateStringFormat);
                    ws.Cells[row, 4].Value = reconciliation.Type.GetDisplayName();
                    ws.Cells[row, 5].Value = reconciliation.Amount;
                    row++;
                    index++;
                };
            });
            return result;
        }
    }
}