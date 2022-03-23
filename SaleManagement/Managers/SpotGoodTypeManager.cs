using Dickson.Core.ComponentModel;
using EntityFramework.Extensions;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class SpotGoodTypeManager : BaseManager
    {
        public SpotGoodTypeManager() : base()
        {

        }

        public SpotGoodTypeManager(SaleUser user) : base(user)
        {

        }

        public async Task<Paging<SpotGoodType>> GetSpotGoodTypeListAsync(int start, int take, Func<IQueryable<SpotGoodType>, IQueryable<SpotGoodType>> filter = null)
        {
            var query = DbContext.Set<SpotGoodType>().AsQueryable();
            if (filter != null)
            {
                query = filter(query);
            }

            var futureCount = query.FutureCount();
            var list = await query.OrderByDescending(u => u.Id).Skip(start).Take(take).ToListAsync();

            return new Paging<SpotGoodType>(start, take, futureCount.Value, list);
        }

        public async Task<List<SpotGoodType>> GetSpotGoodTypeListAsync()
        {
            return await DbContext.Set<SpotGoodType>().Where(a => a.IsDelete == false).ToListAsync(); ;
        }


        public async Task<InvokedResult> SaveSpotGoodType(SpotGoodType spotGoodType)
        {
            DbContext.Set<SpotGoodType>().AddOrUpdate(spotGoodType);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<SpotGoodType> GetSpotGoodType(string id)
        {
            return await DbContext.Set<SpotGoodType>().FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
