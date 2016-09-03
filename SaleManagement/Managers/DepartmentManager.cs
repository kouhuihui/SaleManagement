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
   public class DepartmentManager: BaseManager
    {
        public DepartmentManager(SaleUser user)
        {
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await DbContext.Set<Department>().OrderBy(d=>d.order).ToListAsync();
        }

        public async Task<InvokedResult> CreateAsync(Department department)
        {
            Requires.NotNull(department, nameof(department));

            DbContext.Set<Department>().Add(department);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }
    }
}
