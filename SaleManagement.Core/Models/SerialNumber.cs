using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class SerialNumber
    {
        public SerialNumber()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        public int Id { get; set; }

        public int SN { get; set; }

        public int CompanyId { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.GeneralShorterStringLength)]
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

    }
}
