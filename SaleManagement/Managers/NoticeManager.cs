using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class NoticeManager : BaseManager
    {
        public NoticeManager(SaleUser user) : base(user)
        {

        }

        public async Task<Paging<Notice>> GetNoticesAsync(int start, int take)
        {
            var query = DbContext.Set<Notice>().Where(d => d.CompanyId == User.CompanyId);
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).Skip(start).Take(take).ToListAsync();

            return new Paging<Notice>(start, take, total, list);
        }

        public async Task<InvokedResult> SaveNotice(Notice notice)
        {
            notice.CompanyId = User.CompanyId;
            notice.Created = DateTime.Now;
            notice.CreatorId = User.Id;
            DbContext.Set<Notice>().AddOrUpdate(notice);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<Notice> GetNoticeAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return await DbContext.Set<Notice>().FirstOrDefaultAsync(r => r.Id == id);
        }


        public async Task<Notice> GetNewNoticeAsync()
        {
            return await DbContext.Set<Notice>().OrderByDescending(n => n.Created).FirstOrDefaultAsync();
        }

        public async Task<InvokedResult> DeleteNoticeAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var notice = DbContext.Set<Notice>().FirstOrDefault(r => r.Id == id);
            DbContext.Set<Notice>().Remove(notice);
            await DbContext.SaveChangesAsync();

            return InvokedResult.SucceededResult;
        }
    }
}
