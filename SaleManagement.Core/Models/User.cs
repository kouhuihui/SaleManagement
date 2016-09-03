using Dickson.Library.Security.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class User : IUser, Microsoft.AspNet.Identity.IUser
    {
        public string Id { get; set; }
   
        public string UserName { get; set; }

        public IEnumerable<Claim> GetClaims()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, Id));
            claims.Add(new Claim(ClaimTypes.Name, UserName));
            return claims;
        }

        public User(ClaimsPrincipal principal)
        {
            Id = principal.GetClaimTypeValue(ClaimTypes.NameIdentifier);
            UserName = principal.GetClaimTypeValue(ClaimTypes.Name);
        }
    }
}
