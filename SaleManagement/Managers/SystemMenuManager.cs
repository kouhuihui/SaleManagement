using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class SystemMenuManager : BaseManager
    {
        public async Task<ICollection<SystemMenu>> GetSystemMenusAsync()
        {
            return await DbContext.Set<SystemMenu>().ToListAsync();
        }
    }
}
