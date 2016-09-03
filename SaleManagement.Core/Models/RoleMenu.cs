using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class RoleMenu
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public int SystemMenuId { get; set; }

        public virtual SystemMenu SystemMenu { get; set; }
    }
}
