using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class Reconciliation
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CustomerId { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string CustomerName { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CreatorId { get; set; }

        public int CompanyId { get; set; }

        public DateTime Created { get; set; }

        public ReconciliationType Type { get; set; }
    }
}
