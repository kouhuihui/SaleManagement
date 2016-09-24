using Microsoft.Owin;
using SaleManagement.Managers;
using SaleManagement.Protal.Models;
using SaleManagement.Protal.Web;
using SaleManagement.Store;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class AccountController : AnonymousController
    {
        public ActionResult Login(string returnUrl)
        {
            var isAuthenticateduser = OwinContext.Authentication.User.Identity.IsAuthenticated;
            if (isAuthenticateduser)
            {
                return RedirectToLocal(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var manager = new SignInManager(new SaleUserStore());
            IOwinContext owinContext = HttpContext.GetOwinContext();
            var result = await manager.PasswordSignInAsync(owinContext.Authentication, model.UserName, model.Password, false);
            switch (result)
            {
                case SignInResult.Success:
                    return RedirectToLocal(returnUrl);
                case SignInResult.LockedOut:
                    return View("Lockout");
                case SignInResult.Failure:
                default:
                    ModelState.AddModelError("", "账号或密码不正确");
                    return View(model);
            }
        }

        public ActionResult LogOut()
        {
            OwinContext.Authentication.SignOut();
            return RedirectToLocal("~/");
        }
    }
}