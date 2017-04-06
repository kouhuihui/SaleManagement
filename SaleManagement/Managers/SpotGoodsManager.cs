﻿using Dickson.Core.ComponentModel;
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
    public class SpotGoodsManager : BaseManager
    {
        public SpotGoodsManager() : base()
        {

        }

        public SpotGoodsManager(SaleUser user) : base(user)
        {

        }

        public async Task<IEnumerable<SpotGoodsPattern>> GetSpotGoodsPatternListAsync(SpotGoodsType type)
        {
            return await DbContext.Set<SpotGoodsPattern>().Where(r => r.Type == type).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<string>> GetSpotGoodsMainStoneListAsync(string patternId, int colorFromId)
        {
            return await DbContext.Set<SpotGoods>().Where(r => r.SpotGoodsPatternId == patternId && r.ColorForm.Id == colorFromId && r.Status == SpotGoodsStatus.New).AsNoTracking().Select(r => r.MainStone).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<ColorForm>> GetSpotGoodsColorFromListAsync(string patternId)
        {
            return await DbContext.Set<SpotGoods>().Where(r => r.SpotGoodsPatternId == patternId && r.Status == SpotGoodsStatus.New).AsNoTracking().Select(r => r.ColorForm).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<int>> GetSpotGoodsHandSizeListAsync(string patternId, int colorFromId, string mainStone)
        {
            return await DbContext.Set<SpotGoods>().Where(r => r.SpotGoodsPatternId == patternId && r.MainStone == mainStone && r.ColorForm.Id == colorFromId && r.Status == SpotGoodsStatus.New).AsNoTracking().Select(r => r.HandSize).Distinct().ToListAsync();
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

        public async Task<SpotGoods> GetSpotGoodsAsync(Func<IQueryable<SpotGoods>, IQueryable<SpotGoods>> filter = null)
        {
            var query = DbContext.Set<SpotGoods>().AsQueryable();
            if (filter != null)
            {
                query = filter(query);
            }

            var spotGoods = await query.FirstOrDefaultAsync();

            return spotGoods;
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

        public async Task<InvokedResult> DeleteSpotGoodsSetStoneInfo(string spotGoodsId, int id)
        {
            await DbContext.Set<SpotGoodsSetStoneInfo>().Where(r => r.Id == id & r.SpotGoodsId == spotGoodsId).DeleteAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<SpotGoodsSetStoneInfo> AddSpotGoodsSetStoneInfo(SpotGoodsSetStoneInfo spotGoodsSetStoneInfo)
        {
            DbContext.Set<SpotGoodsSetStoneInfo>().AddOrUpdate(spotGoodsSetStoneInfo);
            await DbContext.SaveChangesAsync();
            return spotGoodsSetStoneInfo;
        }
    }
}
