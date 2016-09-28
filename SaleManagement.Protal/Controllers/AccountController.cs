using Dickson.Web.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Owin;
using SaleManagement.Core;
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
        [WeChatAttribute]
        public ActionResult Login(string returnUrl)
        {
            var isAuthenticateduser = OwinContext.Authentication.User.Identity.IsAuthenticated;
            if (isAuthenticateduser)
                return RedirectToLocal(returnUrl);

            ViewBag.ReturnUrl = returnUrl;



            var isWeChat = OwinContext.GetBrowser().IsWeChat;
            if (isWeChat)
                return View("weChatLogin");

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
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
                case SignInResult.Disabled:
                    ModelState.AddModelError("", "账号已被禁用");
                    return View(model);
                case SignInResult.Failure:
                default:
                    ModelState.AddModelError("", "账号或密码不正确");
                    return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> WeChatLogin(LoginViewModel model, string returnUrl)
        {
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
                return View(model);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var manager = new SignInManager(new SaleUserStore());
            IOwinContext owinContext = HttpContext.GetOwinContext();
            var wxAccount = owinContext.Request.Cookies[SaleManagentConstants.ConfigKeys.wxAccountCookie];
            if (string.IsNullOrEmpty(wxAccount))
            {
                ModelState.AddModelError("", "登陆失败");
                return View("login");
            }
            var accountBindingManager = new AccountBindingManager();
            var isBinding =await accountBindingManager.IsBindingAsync(model.UserName);
            if (isBinding)
            {
                ModelState.AddModelError("", "账号已被绑定,不能重复绑定");
                return View(model);
            }

            var result = await manager.UserNameSignInAsync(owinContext.Authentication, model.UserName, false);
            switch (result)
            {
                case SignInResult.Success:
                    await accountBindingManager.CreateAccountBinding(new Core.Models.AccountBinding
                    {
                        UserName = model.UserName,
                        WxAccount = wxAccount
                    });
                    return RedirectToLocal(returnUrl);
                case SignInResult.LockedOut:
                    return View("Lockout");
                case SignInResult.Disabled:
                    ModelState.AddModelError("", "账号已被禁用");
                    return View(model);
                case SignInResult.Failure:
                default:
                    ModelState.AddModelError("", "账号或手机号码不正确");
                    return View(model);
            }
        }

        public ActionResult LogOut()
        {
            OwinContext.Authentication.SignOut();
           // OwinContext.Response.Cookies.Delete(SaleManagentConstants.ConfigKeys.wxAccountCookie, new CookieOptions { Path = "/" });
            return RedirectToLocal("~/");
        }
    }
}