﻿using Microsoft.Owin;
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

        public static WebBrowser GetBrowser(this IOwinContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var key = typeof(WebBrowser).FullName;
            var browser = context.Get<WebBrowser>(key);
            if (browser == null)
            {
                var agent = context.Request.Headers["User-Agent"];
                browser = new WebBrowser(agent);
                context.Set(key, browser);
            }
            return browser;
        }
    }
}
