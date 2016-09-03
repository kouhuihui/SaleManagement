using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.GeneralShorterStringLength)]
        public string Code { get; set; }

        public RoleType Type { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string Description { get; set; }

        public bool Deleted { get; set; }

        public bool IsSystemRole { get; set; }
    }
}
