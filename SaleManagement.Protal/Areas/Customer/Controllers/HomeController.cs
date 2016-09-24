using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Areas.Customer.Controllers
{
    public class HomeController : PortalController
    {
        // GET: Customer/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}