using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Reconciliation
{
    public class ReconciliationQueryRequest: PagingRequest
    {
        public string CustomerId { get; set; }

        public DateTime? CreatedStartDate { get; set; }

        public DateTime? CreatedEndDate { get; set; }

        public ReconciliationType? Type { get; set; }

        public Func<IQueryable<Core.Models.Reconciliation>, IQueryable<Core.Models.Reconciliation>> GetReconciliationListQueryFilter()
        {
            Func<IQueryable<Core.Models.Reconciliation>, IQueryable<Core.Models.Reconciliation>> filter = query =>
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    query = query.Where(f => f.CustomerId == CustomerId);
                }

                if (CreatedStartDate.HasValue)
                {
                    query = query.Where(f => f.Created >= CreatedStartDate.Value);
                }

                if (CreatedEndDate.HasValue)
                {
                    var endate = CreatedEndDate.Value.AddDays(1);
                    query = query.Where(f => f.Created <= endate);
                }

                if (Type.HasValue)
                {
                    query = query.Where(f => f.Type == Type.Value);
                }
                return query.AsNoTracking();
            };
            return filter;
        }
    }
}