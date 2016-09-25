using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Store;
using System.Security.Claims;

namespace SaleManagement.Managers
{
   public class SignInManager: SignInManager<SaleUser>
    {
        public SignInManager(UserManager manager) : base(manager)
        {
        }
        public SignInManager(SaleUserStore store)
          : base(store)
        {
        }
    }

    public class SignInManager<TUser> where TUser:SaleUser
    {
        UserManager<TUser> m_UserManager;

        public SignInManager(UserManager<TUser> manager)
        {   
            m_UserManager = manager;
        }

        public SignInManager(SaleUserStore<TUser> store)
        {
            m_UserManager = new UserManager<TUser>(store);
        }

        public virtual async Task<SignInResult> PasswordSignInAsync(IAuthenticationManager manager, string userName, string password, bool isPersistent)
        {
            var user = await m_UserManager.FindByNameAsync(userName);
            if (user.Status != UserStatus.Normal)
                return SignInResult.Disabled;

            var result = await PasswordSignInAsync(manager, password, isPersistent, user);

            return result;
        }

        async Task<SignInResult> PasswordSignInAsync(IAuthenticationManager manager, string password, bool isPersistent, TUser user)
        {
            if (user == null)
                return SignInResult.Failure;

            if (await m_UserManager.IsLockedOutAsync(user.Id))
                return SignInResult.LockedOut;

            if (await m_UserManager.CheckPasswordAsync(user, password))
            {
                await m_UserManager.ResetAccessFailedCountAsync(user.Id);
                //switch (user.Status)
                //{
                //    case HeadhuntingUserStatus.Disabled:
                //        return SignInResult.Disabled;
                //    case HeadhuntingUserStatus.Unverified:
                //        return SignInResult.Unverified;
                //    case HeadhuntingUserStatus.Rejected:
                //        return SignInResult.Rejected;
                //    case HeadhuntingUserStatus.Normal:
                //        break;
                //    default:
                //        throw new NotSupportedException("不受支持的用户状态");
                //}

                SignIn(manager, user, isPersistent);
                return SignInResult.Success;
            }

            await m_UserManager.AccessFailedAsync(user.Id);
            if (await m_UserManager.IsLockedOutAsync(user.Id))
                return SignInResult.LockedOut;

            return SignInResult.Failure;
        }

        public virtual void SignIn(IAuthenticationManager manager, TUser user, bool isPersistent)
        {
            manager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            var identity = new ClaimsIdentity(user.GetUserClaims(), DefaultAuthenticationTypes.ApplicationCookie);
            var authAuthenticationCookieExpiresInDays = 7;
            var properties = new AuthenticationProperties { IsPersistent = isPersistent, ExpiresUtc = DateTimeOffset.Now.AddDays(authAuthenticationCookieExpiresInDays) };
            manager.SignIn(properties, identity);
            manager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}
