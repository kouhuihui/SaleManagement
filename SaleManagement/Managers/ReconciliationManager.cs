using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class ReconciliationManager : BaseManager
    {
        public ReconciliationManager()
        {

        }

        public ReconciliationManager(SaleUser user) : base(user)
        {

        }

        public async Task<InvokedResult> CreateAsync(Reconciliation reconciliation)
        {
            DbContext.Set<Reconciliation>().Add(reconciliation);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<Paging<Reconciliation>> GetReconciliationsAsync(int start, int take, Func<IQueryable<Reconciliation>, IQueryable<Reconciliation>> filter = null)
        {
            var query = DbContext.Set<Reconciliation>().Where(o => o.CompanyId == User.CompanyId);
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).Skip(start).Take(take).ToListAsync();

            return new Paging<Reconciliation>(start, take, total, list);
        }

        public async Task<IEnumerable<Reconciliation>> GetCustomerReconciliationsAsync(Func<IQueryable<Reconciliation>, IQueryable<Reconciliation>> filter = null)
        {
            var query = DbContext.Set<Reconciliation>().Where(o => o.CustomerId == User.Id);
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).ToListAsync();

            return list;
        }
    }
}
