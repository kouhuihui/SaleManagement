using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dickson.Library.Security.Principal
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetClaimVersion(this ClaimsPrincipal principal)
        {
            int version = 1;
            var claim = principal.FindFirst(BaseClaimTypes.Version);
            if (claim != null)
            {
                version = int.Parse(claim.Value);
            }
            return version;
        }

        public static string GetClaimTypeValue(this ClaimsPrincipal principal, string type)
        {
            var claim = principal.FindFirst(type);
            if (claim == null)
                throw new Exception("无法找到指定的[" + type + "]类型");

            return claim.Value;
        }
    }
}
