using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;

namespace SaleManagement.Protal.Models.SpotGoodsPattern
{
    public class SpotGoodPatternPageRequest : PagingRequest
    {
        public string SpotGoodTypeId { get; set; }

        public Func<IQueryable<Core.Models.SpotGoodsPattern>, IQueryable<Core.Models.SpotGoodsPattern>> GetSpotGoodsPatternListQueryFilter(SaleUser user)
        {
            Func<IQueryable<Core.Models.SpotGoodsPattern>, IQueryable<Core.Models.SpotGoodsPattern>> filter = query =>
            {

                if (!string.IsNullOrEmpty(SpotGoodTypeId))
                {
                    query = query.Where(f => f.SpotGoodTypeId == SpotGoodTypeId);
                }


                return query.AsNoTracking();
            };
            return filter;
        }
    }
}