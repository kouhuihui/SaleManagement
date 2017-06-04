using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;

namespace SaleManagement.Protal.Models.SpotGoods
{
    public class SpotGoodsQueryRequest : PagingRequest
    {
        public SpotGoodsStatus? Status { get; set; }

        public string OrderIds { get; set; }


        public Func<IQueryable<Core.Models.SpotGoods>, IQueryable<Core.Models.SpotGoods>> GetSpotGoodsListQueryFilter()
        {
            Func<IQueryable<Core.Models.SpotGoods>, IQueryable<Core.Models.SpotGoods>> filter = query =>
            {
                if (!string.IsNullOrEmpty(OrderIds))
                {
                    var orderIds = OrderIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    query = query.Where(f => orderIds.Any(o => f.Id.Contains(o)));
                }

                if (Status.HasValue)
                {
                    query = query.Where(f => f.Status == Status);
                }

                return query.AsNoTracking();
            };
            return filter;
        }

        public Func<IQueryable<Core.Models.SpotGoodsOrder>, IQueryable<Core.Models.SpotGoodsOrder>> GetSpotGoodsOrderListQueryFilter()
        {
            Func<IQueryable<Core.Models.SpotGoodsOrder>, IQueryable<Core.Models.SpotGoodsOrder>> filter = query =>
            {
                if (!string.IsNullOrEmpty(OrderIds))
                {
                    var orderIds = OrderIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    query = query.Where(f => orderIds.Any(o => f.SpotGoods.Id.Contains(o)));
                }

                if (Status.HasValue)
                {
                    query = query.Where(f => f.SpotGoods.Status == Status);
                }

                return query.AsNoTracking();
            };
            return filter;
        }

    }
}