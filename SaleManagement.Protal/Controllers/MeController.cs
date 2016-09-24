using SaleManagement.Managers;
using SaleManagement.Protal.Models.Me;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class MeController : PortalController
    {
        // GET: Me
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


        [HttpPost]
        public async Task<ActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false, "", ErrorToDictionary());

            var manager = new UserManager();
            var user = await manager.FindByIdAsync(User.Id);
            user.Name = model.Name;
            user.Mobile = model.Mobile;
            user.Email = model.Email;
            var result = await manager.UpdateAsync(user);
            if (result.Succeeded)
            {
                manager.RemoveCachedUserItem(user.Id);
            }

            return Json(result.Succeeded, "", result.Errors);
        }

        [HttpPost]
        public async Task<ActionResult> Password(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var manager = new UserManager();
                var result = await manager.ChangePasswordAsync(User.Id, model.CurrentPassword, model.NewPassword);
                return Json(result.Succeeded, "", result.Errors);
            }
            return Json(false, "", ErrorToDictionary());
        }
    }
}