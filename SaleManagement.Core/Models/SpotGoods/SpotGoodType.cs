using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class SpotGoodType
    {

        public SpotGoodType()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        [Required]
        public bool IsDelete { get; set; }


    }
}
