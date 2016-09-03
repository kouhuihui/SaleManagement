using SaleManagement.Core.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class RoleManager : BaseManager
    {
        public RoleManager(SaleUser user)
        {
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await DbContext.Set<Role>().Where(r => !r.Deleted).ToListAsync();
        }

        public async Task<Role> GetRoleAsync(string code)
        {
            return await DbContext.Set<Role>().FirstOrDefaultAsync(r => r.Code == code == !r.Deleted);
        }
    }
}
