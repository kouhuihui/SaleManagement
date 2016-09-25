using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Customer
{
    public class CustomerQueryRequest: PagingRequest
    {
        public string UserName { get; set; }

        public UserStatus? Status { get; set; }

        public Func<IQueryable<Core.Models.SaleUser>, IQueryable<Core.Models.SaleUser>> GetCustomerListQueryFilter(SaleUser User)
        {
            Func<IQueryable<Core.Models.SaleUser>, IQueryable<Core.Models.SaleUser>> filter = query =>
            {
                query = query.Where(f => f.CompanyId == User.CompanyId);
                if (!string.IsNullOrEmpty(UserName))
                {
                    query = query.Where(f => f.Name.Contains(UserName));
                }

                if (Status.HasValue)
                {
                    query = query.Where(f => f.Status == Status.Value);
                }
                return query;
            };
            return filter;
        }

    }
}