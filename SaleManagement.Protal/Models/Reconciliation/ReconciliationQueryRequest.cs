using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Reconciliation
{
    public class ReconciliationQueryRequest : PagingRequest
    {
        public ReconciliationQueryRequest()
        {
            DateTime now = DateTime.Now;
            CreatedStartDate = new DateTime(now.Year, now.Month, 1);
            CreatedEndDate = CreatedStartDate.AddMonths(1).AddDays(-1);
        }

        public string CustomerId { get; set; }

        public DateTime CreatedStartDate { get; set; }

        public DateTime CreatedEndDate { get; set; }

        public ReconciliationType? Type { get; set; }

        public ArrearageType? ArrearageType { get; set; }

        public Func<IQueryable<Core.Models.Reconciliation>, IQueryable<Core.Models.Reconciliation>> GetReconciliationListQueryFilter()
        {
            Func<IQueryable<Core.Models.Reconciliation>, IQueryable<Core.Models.Reconciliation>> filter = query =>
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    query = query.Where(f => f.CustomerId == CustomerId);
                }

                query = query.Where(f => f.Created > CreatedStartDate);

                var endate = CreatedEndDate.AddDays(1);
                query = query.Where(f => f.Created < endate);

                if (Type.HasValue)
                {
                    query = query.Where(f => f.Type == Type.Value);
                }

                if (ArrearageType.HasValue) {
                    if(ArrearageType.Value == Reconciliation.ArrearageType.New) {
                        query = query.Where(f =>f.Type== ReconciliationType.Arrearage && f.Remark.Contains("出货"));
                    }
                    else
                    {
                        query = query.Where(f => f.Type == ReconciliationType.Arrearage && !f.Remark.Contains("出货"));
                    }
                }

                return query.AsNoTracking();
            };
            return filter;
        }
    }
}