using Dickson.Web.Mvc.ModelBinding;
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
        public async Task<ActionResult> List(PagingRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new ReconciliationManager(User);

            var paging = await manager.GetReconciliationsAsync(request.Start, request.Take, null);
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
            return Json(result);
        }
    }
}