using Dickson.Core.ComponentModel;
using Dickson.Library.Security.Principal;
using Dickson.Web.Extensions;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Dickson.Web.Mvc
{
    public class BaseControler : Controller
    {
        IOwinContext m_OwinContext;

        public IOwinContext OwinContext
        {
            get
            {
                if (m_OwinContext == null)
                {
                    m_OwinContext = Request.GetOwinContext();
                }
                return m_OwinContext;
            }
            set
            {
                m_OwinContext = value;
            }
        }

        protected virtual ActionResult RedirectToLocal(string returnUrl = null)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        protected JsonResult Json(bool succeeded, string message = null, object data = null, string contentType = "application/json")
        {
            return this.Json((object)new AjaxResponse<object>(succeeded, message, data), contentType, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            JsonNetResult jsonNetResult = new JsonNetResult();
            object obj = data;
            jsonNetResult.Data = obj;
            string str = contentType;
            jsonNetResult.ContentType = str;
            Encoding encoding = contentEncoding;
            jsonNetResult.ContentEncoding = encoding;
            int num = (int)behavior;
            jsonNetResult.JsonRequestBehavior = (JsonRequestBehavior)num;
            return (JsonResult)jsonNetResult;
        }
        protected JsonResult Json(InvokedResult result, string contentType = "application/json")
        {
            Requires.NotNull(result, "result");
            string message = result.Error != null ? result.Error.Description : string.Empty;
            return this.Json((object)new AjaxResponse<IReadOnlyCollection<ErrorDescriber>>(result.Succeeded, message, result.Errors), contentType, JsonRequestBehavior.AllowGet);
        }


        protected virtual Dictionary<string, string[]> ErrorToDictionary()
        {
            return ModelState.ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
        }
    }

    public abstract class BaseController<TKey, TUser> : BaseControler, IPrincipal<TKey, TUser> where TUser : class, IUser<TKey>
    {
        TUser m_User;
        public new TUser User
        {
            get
            {
                if (m_User == null)
                {
                    m_User = OwinContext.GetAppUser<TUser>();
                }
                return m_User;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                OwinContext.SetAppUser(value);
                m_User = value;
            }
        }
    }
}