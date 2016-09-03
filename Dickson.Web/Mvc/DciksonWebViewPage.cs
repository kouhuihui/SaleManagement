using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dickson.Web.Mvc
{
   public abstract class DciksonWebViewPage<TModel>: WebViewPage<TModel>
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
                if (value == null)
                    throw new ArgumentNullException("value");

                m_OwinContext = value;
            }
        }
    }
}
