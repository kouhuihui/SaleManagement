using Dickson.Library.Security.Principal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace SaleManagement.Core.Models
{
    public class SaleUser : IUser, Microsoft.AspNet.Identity.IUser
    {
        public static readonly int CurrentClaimsVersion = 1;

        public SaleUser()
        {
            Id = Guid.NewGuid().ToString();
            PasswordHash = string.Empty;
            Mobile = string.Empty;
            Telephone = string.Empty;
            Created = DateTime.Now;
            Email = string.Empty;
        }


        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.EmailStringLength)]
        public string UserName { get; set; }

        [StringLength(SaleManagentConstants.Validations.PhoneStringLength)]
        public string Telephone { get; set; }

        [StringLength(SaleManagentConstants.Validations.MoblieStringLength)]
        public string Mobile { get; set; }

        [StringLength(SaleManagentConstants.Validations.EmailStringLength)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [Required(AllowEmptyStrings = true), StringLength(SaleManagentConstants.Validations.PasswordHashStringLength)]
        public string PasswordHash { get; set; }

        [Required(AllowEmptyStrings = true), StringLength(128)]
        public string SecurityStamp { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Created { get; set; }

        public UserStatus Status { get; set; }

        public IdentityType IdentityType { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public ICollection<Claim> GetUserClaims()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(SaleManagementClaimTypes.Version, CurrentClaimsVersion.ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, Id));
            claims.Add(new Claim(ClaimTypes.Name, Name));
            claims.Add(new Claim(SaleManagementClaimTypes.CompanyId, CompanyId.ToString()));
            claims.Add(new Claim(SaleManagementClaimTypes.IdentityType, IdentityType.ToString()));
            return claims;
        }
    }
}
