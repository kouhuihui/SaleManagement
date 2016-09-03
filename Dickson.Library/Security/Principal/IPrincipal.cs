using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dickson.Library.Security.Principal
{
    public interface IPrincipal<TKey, TUser> where TUser : IUser<TKey>
    {
        /// <summary>
        /// 当前主体。
        /// </summary>
        TUser User { get; set; }
    }

    public interface IPrincipal<TUser> : IPrincipal<string, TUser> where TUser : IUser<string>
    {
    }
}
