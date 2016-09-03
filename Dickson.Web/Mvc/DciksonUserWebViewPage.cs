using Dickson.Library.Security.Principal;
using Dickson.Web.Extensions;
using Dickson.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dickson.Web.Mvc
{
    public abstract class DciksonUserWebViewPage<TModel, TKey, TUser> : DciksonWebViewPage<TModel>, IPrincipal<TKey, TUser> where TUser : class, IUser<TKey>
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
                throw new NotImplementedException("User setter not implemented.");
            }
        }
    }
}
