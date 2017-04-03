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
    public class SpotGoodsPatternManager : BaseManager
    {
        public SpotGoodsPatternManager() : base()
        {

        }

        public SpotGoodsPatternManager(SaleUser user) : base(user)
        {

        }

        public async Task<Paging<SpotGoodsPattern>> GetSpotGoodsPatternListAsync(int start, int take, Func<IQueryable<SpotGoodsPattern>, IQueryable<SpotGoodsPattern>> filter = null)
        {
            var query = DbContext.Set<SpotGoodsPattern>().AsQueryable();
            if (filter != null)
            {
                query = filter(query);
            }

            var futureCount = query.FutureCount();
            var list = await query.OrderByDescending(u => u.Id).Skip(start).Take(take).ToListAsync();

            return new Paging<SpotGoodsPattern>(start, take, futureCount.Value, list);
        }

        public async Task<InvokedResult> SaveSpotGoodsPattern(SpotGoodsPattern spotGoodsPattern)
        {
            DbContext.Set<SpotGoodsPattern>().AddOrUpdate(spotGoodsPattern);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<SpotGoodsPattern> GetSpotGoodsPattern(string Id)
        {
            return await DbContext.Set<SpotGoodsPattern>().FirstOrDefaultAsync(r => r.Id == Id);
        }
    }
}
