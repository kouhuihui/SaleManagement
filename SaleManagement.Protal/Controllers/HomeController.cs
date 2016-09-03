using Dickson.Web.Extensions;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Home;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class HomeController : PortalController
    {
        public async Task<ActionResult> Index()
        {
            if (User.IdentityType == IdentityType.Customer)
                return RedirectToAction("Index", "Home", new { Area = "Customer" });

            var manager = new OrderManager(User);
            var statics = await manager.GetOrderStatisticsAsync();
            var viewModel = new HomeViewModel
            {
                OrderStatistics = statics
            };
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}