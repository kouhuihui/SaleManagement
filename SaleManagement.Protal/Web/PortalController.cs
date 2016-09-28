using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaleManagement.Core.Models;
using Dickson.Web.Mvc;
using System.Diagnostics;
using SaleManagement.Core;
using System.Data.Entity.Validation;
using Microsoft.Extensions.Logging;

namespace SaleManagement.Protal.Web
{
    public abstract class AnonymousController : BaseController<string, SaleUser>
    {
        protected new ActionResult RedirectToLocal(string returnUrl = null)
        {
            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            LoggerHelper.Logger.LogError(string.Format("请求发生异常\r\nUrl:{0}\r\nController:{1}\r\nAction:{2}\r\n{3}",
                filterContext.HttpContext.Request.RawUrl,
                filterContext.RouteData.GetRequiredString("controller"),
                filterContext.RouteData.GetRequiredString("action"),filterContext.Exception.Message),
                filterContext.Exception);

            var validationException = filterContext.Exception as DbEntityValidationException;
            if (validationException != null)
            {
                foreach (var result in validationException.EntityValidationErrors)
                {
                    LoggerHelper.Logger.LogError(string.Join(",", result.ValidationErrors.Select(e => e.ErrorMessage).ToArray()));
                }
            }

            if (!filterContext.ExceptionHandled)
            {
                var IsArgException = typeof(ArgumentException).IsAssignableFrom(filterContext.Exception.GetType());
                filterContext.Result = Error(IsArgException ? "参数错误" : "操作失败，请联系管理员。");
                filterContext.ExceptionHandled = true;
            }
        }

        protected virtual ActionResult Error(string format = null, params string[] args)
        {
            var message = string.IsNullOrWhiteSpace(format) ?"操作失败，请稍后重试。" :format;
            return new ErrorResult(message, "Error");
        }
    }

    [PortalAuthenticate]
    public abstract class PortalController : AnonymousController
    {
    }
}