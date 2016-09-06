using Dickson.Library.Security.Principal;
using Dickson.Web.Extensions;
using Dickson.Web.Mvc;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Protal.Models;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc.Filters;

namespace SaleManagement.Protal.Web
{
    public class PortalAuthenticateAttribute : BaseAuthenticateAttribute
    {
        readonly bool m_HandleUnauthorizedRequest;

        public PortalAuthenticateAttribute(bool handleUnauthorizedRequest = true)
        {
            m_HandleUnauthorizedRequest = handleUnauthorizedRequest;
        }

        protected override bool TryGetUser(IPrincipal principal, out IUser user)
        {
            user = null;
            try
            {
                user = new User((ClaimsPrincipal)principal);
                var manager = new UserManager();
                user = manager.FindByIdByCache(user.Id);
                return user != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected override void OnAuthenticated(AuthenticationContext filterContext, IUser user)
        {
            var userModel = (SaleUser)user;
            filterContext.HttpContext.GetOwinContext().SetAppUser(userModel);
            var userViewModel = new SaleUserViewModel(userModel);
            userViewModel.RoleMenus = new RoleMenuManager().GetRoleMenus(userModel.RoleId);
            filterContext.HttpContext.GetOwinContext().SetAppUser(userViewModel);
        }

        protected override void HandleUnauthorizedRequest(AuthenticationContext filterContext)
        {
            if (m_HandleUnauthorizedRequest)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}