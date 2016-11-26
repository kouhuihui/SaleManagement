using Dickson.Core.ComponentModel;
using Dickson.Library.Security.Principal;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dickson.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BaseAuthorizeAttribute :  FilterAttribute, IAuthorizationFilter
    {
        readonly object m_TypeId = new object();
        public string ActionName;

        public string ControllerName;
        public override object TypeId
        {
            get
            {
                return m_TypeId;
            }
        }

        protected virtual bool AuthorizeCore(IOwinContext owinContext)
        {
            Requires.NotNull(owinContext, "owinContext");

            IPrincipal user = owinContext.Authentication.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            return true;
        }

        void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(context.GetOwinContext());
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            ActionName = filterContext.ActionDescriptor.ActionName;
            ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                // If a child action cache block is active, we need to fail immediately, even if authorization
                // would have succeeded. The reason is that there's no way to hook a callback to rerun
                // authorization before the fragment is served from the cache, so we can't guarantee that this
                // filter will be re-run on subsequent requests.
                throw new InvalidOperationException("BaseAuthorizeAttribute Cannot Use Within Child Action Cache");
            }

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                     || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization)
            {
                return;
            }

            if (AuthorizeCore(filterContext.HttpContext.GetOwinContext()))
            {
                // ** IMPORTANT **
                // Since we're performing authorization at the action level, the authorization code runs
                // after the output caching module. In the worst case this could allow an authorized user
                // to cause the page to be cached, then an unauthorized user would later be served the
                // cached page. We work around this by telling proxies not to cache the sensitive page,
                // then we hook our custom authorization code into the caching mechanism so that we have
                // the final say on whether a page should be served from the cache.

                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
            }
            else
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            filterContext.Result = new HttpUnauthorizedResult();
        }

        // This method must be thread-safe since it is called by the caching module.
        protected virtual HttpValidationStatus OnCacheAuthorization(IOwinContext owinContext)
        {
            Requires.NotNull(owinContext, "owinContext");

            bool isAuthorized = AuthorizeCore(owinContext);
            return (isAuthorized) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }
    }
}
