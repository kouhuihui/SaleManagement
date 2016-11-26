using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class SystemMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        public int Level { get; set; }
        
        public int? parentId {get;set;}

        public int Order { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public string ControllerAction { get; set; }
    }
}
