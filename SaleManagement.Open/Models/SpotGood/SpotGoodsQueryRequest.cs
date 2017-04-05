using SaleManagement.Core;
using SaleManagement.Core.Models;
using System;
using System.Linq;

namespace SaleManagement.Open.Models.SpotGood
{
    public class SpotGoodsQueryRequest
    {
        public string PatternId { get; set; }

        public int ColorFormId { get; set; }

        public string MainStone { get; set; }

        public int HandSize { get; set; }

        public Func<IQueryable<Core.Models.SpotGoods>, IQueryable<Core.Models.SpotGoods>> GetSpotGoodsListQueryFilter()
        {
            Func<IQueryable<Core.Models.SpotGoods>, IQueryable<Core.Models.SpotGoods>> filter = query =>
            {
                query = query.Where(r => r.status == SpotGoodsStatus.New);
                if (!string.IsNullOrEmpty(PatternId))
                {
                    query = query.Where(r => r.SpotGoodsPattern.Id == PatternId);
                }
                if (!string.IsNullOrEmpty(MainStone))
                {
                    query = query.Where(r => r.MainStone == MainStone);
                }
                if (SaleManagentConstants.UI.HandSizes.Contains(HandSize))
                {
                    query = query.Where(r => r.HandSize == HandSize);
                }
                if (ColorFormId > 0)
                {
                    query = query.Where(r => r.ColorForm.Id == ColorFormId);
                }

                return query;
            };

            return filter;
        }
    }
}