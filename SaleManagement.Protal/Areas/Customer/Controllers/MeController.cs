using SaleManagement.Protal.Models.Me;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Areas.Customer.Controllers
{
    public class MeController : PortalController
    {
        // GET: Customer/Me
        public ActionResult Index()
        {
            var profile = new ProfileViewModel
            {
                Mobile = User.Mobile,
                Name = User.Name,
                Telephone = User.Telephone,
                Email = User.Email
            };
            var model = new IndexViewModel
            {
                Profile = profile,
                Password = new ChangePasswordViewModel()
            };
            return View(model);
        }
    }
}