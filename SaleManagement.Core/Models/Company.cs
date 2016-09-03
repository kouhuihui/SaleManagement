using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagement.Core.Models
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]

        public string ShortName { get; set; }

        [StringLength(SaleManagentConstants.Validations.PhoneStringLength)]

        public string Phone { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string Address { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.UserNameStringLength)]
        public string ContactPerson { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]

        public string Remark { get; set; }

        public CompanyStatus Status { get; set; }
    }
}
