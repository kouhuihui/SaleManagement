using SaleManagement.Core.ViewModel;
using System;
using System.Linq;

namespace SaleManagement.Protal.Models.HotSelling
{
    public class HotSellingQueryRequest : PagingRequest
    {
        public int? ProductCategoryId { get; set; }

        public int? GemCategoryId { get; set; }

        public Func<IQueryable<Core.Models.HotSelling>, IQueryable<Core.Models.HotSelling>> GetHotSellingQueryFilter()
        {
            Func<IQueryable<Core.Models.HotSelling>, IQueryable<Core.Models.HotSelling>> filter = query =>
            {
                query = query.Where(f => f.Deleted == false);

                if (ProductCategoryId.HasValue)
                {
                    query = query.Where(f => f.ProductCategoryId == ProductCategoryId.Value);
                }
                if (GemCategoryId.HasValue)
                {
                    query = query.Where(f => f.GemCategoryId == GemCategoryId.Value);
                }

                return query;
            };
            return filter;
        }
    }
}