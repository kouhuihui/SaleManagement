using Dickson.Library.Security.Principal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class CustomerInfo
    {
        public CustomerInfo()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string UserId { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string Address { get; set; }
    }
}
