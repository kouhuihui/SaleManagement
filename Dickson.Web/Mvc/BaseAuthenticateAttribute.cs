using Dickson.Library.Security.Principal;
using Dickson.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Dickson.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class BaseAuthenticateAttribute : FilterAttribute, IAuthenticationFilter
    {
        public bool ShouldHandleUnauthorizedRequest { get; set; }

        public virtual void OnAuthentication(AuthenticationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
                return;

            IUser user;
            var principal = filterContext.Principal;
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated || !TryGetUser(principal, out user))
            {
                HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                OnAuthenticated(filterContext, user);
            }

            if (filterContext.Result != null)
            {
                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
            }
        }

        protected abstract bool TryGetUser(IPrincipal principal, out IUser user);

        protected virtual void HandleUnauthorizedRequest(AuthenticationContext filterContext)
        {
            // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            filterContext.Result = new HttpUnauthorizedResult();//new RedirectResult("/Account/Login?returnUrl="+ filterContext.HttpContext.);
        }

        protected virtual void OnAuthenticated(AuthenticationContext filterContext, IUser user)
        {
            filterContext.HttpContext.GetOwinContext().SetAppUser(user);
        }

        void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            IUser user;
            bool isAuthorized = TryGetUser(httpContext.User, out user);
            return (isAuthorized) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}
