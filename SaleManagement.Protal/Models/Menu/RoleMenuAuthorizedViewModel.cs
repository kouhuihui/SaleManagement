using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Menu
{
    public class RoleMenuAuthorizedViewModel
    {
        public ICollection<SystemMenu> SystemMenus { get; set; }

        public ICollection<RoleMenu> RoleMenus { get; set; }

        public int RoleId { get; set; }
    }
}