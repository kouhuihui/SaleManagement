using SaleManagement.Core.Models;
using SaleManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class BaseManager : SaleManagementSerivce
    {
        public SaleUser User { get; set; }

        public BaseManager()
        {
        }

        public BaseManager(SaleUser user)
        {
            User = user;
        }
    }
}
