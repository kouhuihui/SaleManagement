using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required,StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public int order { get; set; }

        public DateTime Created { get; set; }
    }
}
