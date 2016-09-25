using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class AccountBinding
    {
        public AccountBinding()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        public  string UserName { get; set; }

        public string WxAccount { get; set; }

        public DateTime Created { get; set; }
    }
}
