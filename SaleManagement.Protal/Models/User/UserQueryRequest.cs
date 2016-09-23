using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Linq;

namespace SaleManagement.Protal.Models.User
{
    public class UserQueryRequest : PagingRequest
    {
        public string UserName { get; set; }

        public UserStatus? UserStatus { get; set; }

        public Func<IQueryable<Core.Models.SaleUser>, IQueryable<Core.Models.SaleUser>> GetUseristQueryFilter()
        {
            Func<IQueryable<Core.Models.SaleUser>, IQueryable<Core.Models.SaleUser>> filter = query =>
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    query = query.Where(f => f.UserName.Contains(UserName));
                }

                if (UserStatus.HasValue)
                {
                    query = query.Where(f => f.Status == UserStatus.Value);
                }
                return query;
            };
            return filter;
        }
    }
}