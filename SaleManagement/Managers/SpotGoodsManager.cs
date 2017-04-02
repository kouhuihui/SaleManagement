using Dickson.Core.ComponentModel;
using EntityFramework.Extensions;
using SaleManagement.Core.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class SpotGoodsManager : BaseManager
    {
        public SpotGoodsManager() : base()
        {

        }

        public SpotGoodsManager(SaleUser user) : base(user)
        {

        }

        public async Task<Paging<SpotGoods>> GetSpotGoodsListAsync(int start, int take, Func<IQueryable<SpotGoods>, IQueryable<SpotGoods>> filter = null)
        {
            var query = DbContext.Set<SpotGoods>().AsQueryable();
            if (filter != null)
            {
                query = filter(query);
            }

            var futureCount = query.FutureCount();
            var list = await query.OrderByDescending(u => u.Id).Skip(start).Take(take).ToListAsync();

            return new Paging<SpotGoods>(start, take, futureCount.Value, list);
        }

        public async Task<SpotGoods> GetSpotGoods(string id)
        {
            Requires.NotNullOrEmpty("id", nameof(id));

            return await DbContext.Set<SpotGoods>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<InvokedResult> CreateSpotGoods(SpotGoods spotGoods)
        {
            spotGoods.CreatorId = User.Id;
            DbContext.Set<SpotGoods>().AddOrUpdate(spotGoods);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }
    }
}
