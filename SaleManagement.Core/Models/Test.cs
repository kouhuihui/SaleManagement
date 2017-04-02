using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
   public class Test
    {
        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }
    }
}
