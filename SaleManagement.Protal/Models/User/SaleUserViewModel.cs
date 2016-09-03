using Dickson.Library.Security.Principal;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models
{
    public class SaleUserViewModel: IUser
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Telephone { get; set; }

        public UserStatus Status { get; set; }

        public IdentityType IdentityType { get; set; }

        public int CompanyId { get; set; }

        public Role Role { get; set; }

        public IEnumerable<RoleMenu> RoleMenus { get; set; }

        public SaleUserViewModel(SaleUser user)
        {
            Id = user.Id;
            Name = user.Name;
            UserName = user.UserName;
            Email = user.Email;
            Mobile = user.Mobile;
            Telephone = user.Telephone;
            Status = user.Status;
            IdentityType = user.IdentityType;
            CompanyId = user.CompanyId;
            Role = user.Role;
        }
    }
}