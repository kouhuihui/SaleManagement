using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class DailyGoldPriceManager : BaseManager
    {
        public DailyGoldPriceManager()
        {
        }

        public DailyGoldPriceManager(SaleUser user) : base(user)
        {
        }

        public async Task<Paging<DailyGoldPrice>> GetDailyGoldPrices(int start, int take, DateTime? date = null)
        {
            var query = DbContext.Set<DailyGoldPrice>().Where(d => d.CompanyId == User.CompanyId);
            if (date.HasValue)
            {
                query = query.Where(d => d.Date == date);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).Skip(start).Take(take).ToListAsync();

            return new Paging<DailyGoldPrice>(start, take, total, list);
        }

        public async Task<InvokedResult> SaveDailyGoldPrice(DailyGoldPrice dailyGoldPrice)
        {
            var dbSet = DbContext.Set<DailyGoldPrice>();
            var existDailyGoldPrice = dbSet.FirstOrDefault(d => d.CompanyId == User.CompanyId && d.Date ==dailyGoldPrice.Date && d.ColorFormId == dailyGoldPrice.ColorFormId && dailyGoldPrice.Id != d.Id);
            if (existDailyGoldPrice != null)
                return InvokedResult.Fail("DailyGoldPriceIsExist",
                    $"{existDailyGoldPrice.ColorForm.Name}在{existDailyGoldPrice.Date}的金价已设置,不能重复添加");

            dbSet.AddOrUpdate(dailyGoldPrice);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<DailyGoldPrice> GetDailyGoldPriceAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return await DbContext.Set<DailyGoldPrice>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<DailyGoldPrice> GetDailyGoldPriceAsync(int colorFormId, DateTime date)
        {
            return await DbContext.Set<DailyGoldPrice>().FirstOrDefaultAsync(r => r.Date == date && r.ColorFormId == colorFormId);
        }

        public async Task<InvokedResult> DeleteDailyGoldPriceAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var dailyGoldPrice = DbContext.Set<DailyGoldPrice>().FirstOrDefault(r => r.Id == id);
            DbContext.Set<DailyGoldPrice>().Remove(dailyGoldPrice);
            await DbContext.SaveChangesAsync();

            return InvokedResult.SucceededResult;
        }
    }
}
