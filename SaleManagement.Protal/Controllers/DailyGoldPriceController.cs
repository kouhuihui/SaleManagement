using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.DailyGoldPrice;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class DailyGoldPriceController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(PagingRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new DailyGoldPriceManager(User);
            var paging = await manager.GetDailyGoldPrices(request.Start, request.Take);

            var users = paging.List.Select(u =>
                new DailyGoldPriceViewModel(u));

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = users,
            });
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var manager = new DailyGoldPriceManager(User);
            var dailyGoldPriceViewModel = new DailyGoldPriceViewModel();

            if (id > 0)
            {
                var dailyGoldPrice = await manager.GetDailyGoldPriceAsync(id);
                if (dailyGoldPrice != null)
                {
                    dailyGoldPriceViewModel = new DailyGoldPriceViewModel(dailyGoldPrice);
                }
            }

            var colorFormManager = new BasicDataManager(User);
            dailyGoldPriceViewModel.ColorForms = await colorFormManager.GetColorFormsAsync();

            return View(dailyGoldPriceViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Edit(DailyGoldPriceViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var dailyGoldPrice = new DailyGoldPrice()
            {
                Id = viewModel.Id,
                ColorFormId = viewModel.ColorFormId,
                CompanyId = User.CompanyId,
                CreatorId = User.Id,
                Price = viewModel.Price,
                Date = Convert.ToDateTime(viewModel.Date)
            };
            var manager = new DailyGoldPriceManager(User);
            var result = await manager.SaveDailyGoldPrice(dailyGoldPrice);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var manager = new DailyGoldPriceManager(User);
            var result = await manager.DeleteDailyGoldPriceAsync(id);

            return Json(result);
        }

        public async Task<ActionResult> Add()
        {
            var colorFormManager = new BasicDataManager(User);
            var colorForms = await colorFormManager.GetColorFormsAsync();
            var dailyGoldPriceViewModels = colorForms.Select(c =>
            {
                return new DailyGoldPriceViewModel
                {
                    ColorFormId = c.Id,
                    ColorFormName = c.Name
                };
            });
            return View(dailyGoldPriceViewModels);
        }

        [HttpPost]
        public async Task<JsonResult> Add(IEnumerable<DailyGoldPriceViewModel> dailyGoldPriceViewModels)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var dailyGoldPrices = dailyGoldPriceViewModels.Select(r =>
            {
                return new DailyGoldPrice
                {
                    ColorFormId = r.ColorFormId,
                    CompanyId = User.CompanyId,
                    Date = Convert.ToDateTime(r.Date),
                    Price = r.Price,
                    CreatorId = User.Id
                };
            });
            var manager = new DailyGoldPriceManager(User);
            var result = await manager.SaveDailyGoldPrices(dailyGoldPrices);

            return Json(true);
        }
    }
}