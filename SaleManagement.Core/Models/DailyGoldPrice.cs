using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class DailyGoldPrice
    {
        public DailyGoldPrice()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        public int Id { get; set; }

        public double Price { get; set; }

        public int ColorFormId { get; set; }

        public virtual ColorForm ColorForm { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CreatorId { get; set; }

        public DateTime Created { get; set; }


        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string UpdaterId { get; set; }

        public DateTime Updated { get; set; }

        public int CompanyId { get; set; }
    }
}
