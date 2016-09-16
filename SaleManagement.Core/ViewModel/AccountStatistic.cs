using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.ViewModel
{
    public class AccountStatistic
    {
        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public double SurplusArrearage { get; set; }

        public double PaymentInQuery { get; set; }
    }
}
