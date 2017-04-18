using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class CustomerAddress
    {
        public CustomerAddress()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }

        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string OpenId { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string Address { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string Name { get; set; }

        [StringLength(SaleManagentConstants.Validations.MoblieStringLength)]
        public string Phone { get; set; }

        public DateTime Created { get; set; }

        public bool IsCommonly { get; set; }
    }
}
