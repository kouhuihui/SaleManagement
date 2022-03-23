using Dickson.Core.ComponentModel;
using EntityFramework.Extensions;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
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
            var list = await query.OrderByDescending(u => u.RowNo).Skip(start).Take(take).ToListAsync();

            return new Paging<SpotGoodsPattern>(start, take, futureCount.Value, list);
        }

        public async Task<IEnumerable<SpotGoodsPattern>> GetSpotGoodsPatternListAsync(string typeId)
        {
            var query = DbContext.Set<SpotGoodsPattern>().Where(r => r.SpotGoodsPatternTypes.Any(w => w.SpotGoodsTypeId == typeId)).AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<InvokedResult> SaveSpotGoodsPattern(SpotGoodsPattern spotGoodsPattern, string[] spotGoodTypeIds)
        {
            spotGoodsPattern.SpotGoodsPatternTypes = new List<SpotGoodsPatternType>();

            foreach (var spotGoodTypeId in spotGoodTypeIds)
            {
                spotGoodsPattern.SpotGoodsPatternTypes.Add(new SpotGoodsPatternType
                {
                    Id = Guid.NewGuid().ToString(),
                    SpotGoodsPatternId = spotGoodsPattern.Id,
                    SpotGoodsTypeId = spotGoodTypeId
                });
            }

            DbContext.Set<SpotGoodsPattern>().AddOrUpdate(spotGoodsPattern);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> EditSpotGoodsPattern(SpotGoodsPattern spotGoodsPattern, string[] spotGoodTypeIds)
        {
            var spotGoodsPatternTypes = DbContext.Set<SpotGoodsPatternType>().Where(t => t.SpotGoodsPatternId == spotGoodsPattern.Id).ToList();
            DbContext.Set<SpotGoodsPatternType>().RemoveRange(spotGoodsPatternTypes);

            var spotGoodsPatternTypeList = new List<SpotGoodsPatternType>();

            foreach (var spotGoodTypeId in spotGoodTypeIds)
            {
                spotGoodsPatternTypeList.Add(new SpotGoodsPatternType
                {
                    Id = Guid.NewGuid().ToString(),
                    SpotGoodsPatternId = spotGoodsPattern.Id,
                    SpotGoodsTypeId = spotGoodTypeId
                });
            }

            DbContext.Set<SpotGoodsPattern>().AddOrUpdate(spotGoodsPattern);
            DbContext.Set<SpotGoodsPatternType>().AddRange(spotGoodsPatternTypeList);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }


        public async Task<SpotGoodsPattern> GetSpotGoodsPattern(string Id)
        {
            return await DbContext.Set<SpotGoodsPattern>().FirstOrDefaultAsync(r => r.Id == Id);
        }
    }
}
