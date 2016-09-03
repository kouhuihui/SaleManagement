using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class RoleMenuManager : BaseManager
    {
        public async Task<IEnumerable<RoleMenu>> GetRoleMenusAsync(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentOutOfRangeException(nameof(roleId));

            return await DbContext.Set<RoleMenu>().Where(r => r.RoleId == roleId).ToListAsync();
        }

        public IEnumerable<RoleMenu> GetRoleMenus(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentOutOfRangeException(nameof(roleId));

            return DbContext.Set<RoleMenu>().Where(r => r.RoleId == roleId).ToList();
        }

        public async Task<InvokedResult> SaveRoleMenusAsync(int[] menuIds, int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentOutOfRangeException(nameof(roleId));

            var dbSet = DbContext.Set<RoleMenu>();
            var roleMenus = dbSet.Where(r => r.RoleId == roleId);
            dbSet.RemoveRange(roleMenus);

            if (menuIds != null && menuIds.Any())
            {
                menuIds.ToList().ForEach(m => dbSet.Add(new RoleMenu
                {
                    RoleId = roleId,
                    SystemMenuId = m
                }));
            }

            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }
    }
}
