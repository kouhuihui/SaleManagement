using Dickson.Core.Common.Extensions;
using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class HotSellingManager : BaseManager
    {
        public HotSellingManager()
        {
        }

        public HotSellingManager(SaleUser user) : base(user)
        {
        }

        public async Task<Paging<HotSelling>> GetHotSellingsAsync(int start, int take, Func<IQueryable<HotSelling>, IQueryable<HotSelling>> filter = null)
        {
            var query = DbContext.Set<HotSelling>().AsQueryable();
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.RowNo).Skip(start).Take(take).ToListAsync();

            return new Paging<HotSelling>(start, take, total, list);
        }

        public async Task<InvokedResult> SaveHotSellingAsync(HotSelling hotSelling)
        {
            DbContext.Set<HotSelling>().AddOrUpdate(hotSelling);
            await DbContext.SaveChangesAsync();

            return InvokedResult.SucceededResult;
        }

        public async Task<HotSelling> GetHotSellingAsync(string id)
        {
            var hotSelling = await DbContext.Set<HotSelling>().FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            SetHotSellingAttachement(hotSelling);
            return hotSelling;
        }

        public async Task<HotSelling> GetHotSellingByNoAsync(string versionNo)
        {
            var hotSelling = await DbContext.Set<HotSelling>().FirstOrDefaultAsync(c => c.VersionNo == versionNo && !c.Deleted);

            SetHotSellingAttachement(hotSelling);
            return hotSelling;
        }

        private void SetHotSellingAttachement(HotSelling hotSelling)
        {
            var fileIds = hotSelling.Attachments.Select(t => t.FileInfoId);
            var fileInfos = DbContext.Set<FileInfo>().Where(t => fileIds.Contains(t.Id)).Select(a => new
            {
                Id = a.Id,
                FilePurpose = a.Purpose
            }).ToList();

            hotSelling.Attachments.ToList().ForEach(t =>
            {
                var fileInfo = fileInfos.FirstOrDefault(w => w.Id == t.FileInfoId);
                if (fileInfo != null)
                {
                    t.FileType = fileInfo.FilePurpose == FilePurpose.HotSellingParamAttachment.GetDisplayName() ? 1 : 0;
                }

            });
        }

        public async Task<InvokedResult> DeleteHotSellingAsync(string id)
        {

            var dbSet = DbContext.Set<HotSelling>();
            var hotSelling = await dbSet.FirstOrDefaultAsync(c => c.Id == id);
            if (hotSelling == null)
                return InvokedResult.Fail("HotSellingNoExists", "数据不存在");

            hotSelling.Deleted = true;
            dbSet.AddOrUpdate(hotSelling);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<bool> RemoveAttachment(HotSellingAttachment attachement)
        {
            DbContext.Set<HotSellingAttachment>().Remove(attachement);
            return await DbContext.SaveChangesAsync() > 0;
        }

    }
}
