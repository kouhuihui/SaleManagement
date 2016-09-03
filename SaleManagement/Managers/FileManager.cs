using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class FileManager : BaseManager
    {
        public FileManager()
        {
        }

        public FileManager(SaleUser user)
        {
        }

        public async Task<FileInfo> FindByIdAsync(string id)
        {
            Requires.NotNull(id, "id");
            return await DbContext.Set<FileInfo>().FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<InvokedResult> CreateAsync(FileInfo file)
        {
            Requires.NotNull(file, "file");
            DbContext.Set<FileInfo>().Add(file);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> UpdateAsync(FileInfo file)
        {
            Requires.NotNull(file, "file");
            DbContext.Set<FileInfo>().AddOrUpdate(file);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> DeleteAsync(string id)
        {
            Requires.NotNull(id, "id");
            var file = await FindByIdAsync(id);
            if (file == null)
                return InvokedResult.Fail("404","附件不存在");

            DbContext.Set<FileInfo>().Remove(file);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }
    }
}
