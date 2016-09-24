using Microsoft.Owin;
using SaleManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Web
{
    public class WeChatAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var owinContext = filterContext.HttpContext.GetOwinContext();
            var cookie = owinContext.Request.Cookies[SaleManagentConstants.ConfigKeys.wxAccountCookie];
            if (cookie != null)
                return ;
            
            var wxAccount = owinContext.Request.Query.Get("wxAccount");
            if (!string.IsNullOrEmpty(wxAccount))
            {
                var date = DateTime.Now.Date;
                var options = new CookieOptions { Expires = date.AddDays(SaleManagentConstants.UI.DefaultExpiringDays) };
                owinContext.Response.Cookies.Append(SaleManagentConstants.ConfigKeys.wxAccountCookie,
                    wxAccount, options);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}