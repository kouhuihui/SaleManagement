using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class CustomerDiscountRate
    {
        public CustomerDiscountRate()
        {
            StoneSetter = 100;
            SideStone = 100;
            PriceOfWork = 100;
            Loss18K = 12;
            LossPt = 15;
        }

        public int Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CustomerId { get; set; }

        public virtual SaleUser Customer { get; set; }

        public int StoneSetter { get; set; }

        public int SideStone { get; set; }

        public int PriceOfWork { get; set; }

        public int Loss18K { get; set; }

        public int LossPt { get; set; }

        public DateTime Created { get; set; }

        public string CreatorId { get; set; }
    }
}
