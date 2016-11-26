using Dickson.Web.Extensions;
using Dickson.Web.Mvc;
using Microsoft.Owin;
using SaleManagement.Protal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class UrlAuthorizeAttribute: BaseAuthorizeAttribute
    {
        protected override bool AuthorizeCore(IOwinContext owinContext)
        {
            var authorized = base.AuthorizeCore(owinContext);
            if (!authorized)
                return false;
            var user = owinContext.GetAppUser<SaleUserViewModel>();
            return user.RoleMenus.Any(u =>string.Equals(u.SystemMenu.ControllerAction,$"{ControllerName}_{ActionName}", StringComparison.OrdinalIgnoreCase));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { succeeded = false, message = "权限不足，请联系管理员" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new ErrorResult("权限不足，请联系管理员", "Error");
            }
        }
    }
}