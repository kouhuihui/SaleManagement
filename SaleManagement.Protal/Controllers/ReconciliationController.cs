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
        [UrlAuthorize]
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

        public ActionResult Add()
        {
            var model = new ReconciliationItemViewModel();
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
                return Json(false, "参数错误");

            var manager = new ReconciliationManager(User);
            var reconciliation = await manager.GetReconciliationAsync(id);
            var reconciliationItemViewModel = new ReconciliationItemViewModel(reconciliation);
            return View(reconciliationItemViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Add(ReconciliationItemViewModel request)
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
                Type = request.Type,
                Created = DateTime.Now,
                CreatorId = User.Id,
                Remark = request.Remark
            };
            var result = await manager.CreateAsync(reconciliation);
            return Json(true, string.Empty, result);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var manager = new ReconciliationManager(User);          
            var reconciliation = await manager.GetReconciliationAsync(id);
            if (reconciliation == null)
                return Json(false, "对账记录不存在");

            if (reconciliation.Remark.Contains("出货"))
                return Json(false, "出货单的对账记录只能取消审核出货单，不能直接删除");

            var result = await manager.DeleteReconciliationAsync(reconciliation);
            return Json(true, string.Empty, result); ;
        }

        [HttpPost]
        public async Task<JsonResult> Edit(ReconciliationItemViewModel request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new ReconciliationManager(User);
            var reconciliation =await  manager.GetReconciliationAsync(request.Id);
            reconciliation.Type = request.Type;
            reconciliation.Amount = request.Amount;
            reconciliation.Remark = request.Remark;
          
            var result = await manager.CreateAsync(reconciliation);
            return Json(true, string.Empty, result);
        }

        public async Task<FileStreamResult> Export(ReconciliationQueryRequest request)
        {
            var manager = new ReconciliationManager(User);
            var reconciliations = await manager.GetReconciliationsAsync(request.GetReconciliationListQueryFilter());
            var titles = new string[] { "序号", "客户", "日期", "付/欠款", "金额(元)", "备注" };
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
                    ws.Cells[row, 6].Value = reconciliation.Remark;
                    row++;
                    index++;
                };
            });
            return result;
        }
    }
}