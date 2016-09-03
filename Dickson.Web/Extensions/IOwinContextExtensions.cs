using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dickson.Web.Extensions
{
    public static class IOwinContextExtensions
    {
        static readonly string _AppUserKeyPrefix = "SaleManagement:AppUser:";

        public static TUser GetAppUser<TUser>(this IOwinContext context) where TUser : class
        {
            if (context == null)
                throw new ArgumentNullException("context");

            return context.Get<TUser>(_AppUserKeyPrefix + typeof(TUser).FullName);
        }

        public static void SetAppUser<TUser>(this IOwinContext context, TUser user) where TUser : class
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (user == null)
                throw new ArgumentNullException("user");

            context.Set(_AppUserKeyPrefix + typeof(TUser).FullName, user);
        }
    }
}
