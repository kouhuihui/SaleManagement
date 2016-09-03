using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Customer
{
    public class CustomerViewModel : SaleUserViewModel
    {
        public CustomerViewModel(SaleUser user) : base(user)
        {
        }

        public double StoneSetterDiscountRate { get; set; }
     
        public double SideStoneDiscountRate { get; set; }

        public double PriceOfWorkDiscountRate { get; set; }

        public int Loss18KRate { get; set; }

        public int LossPtRate { get; set; }
    }
}