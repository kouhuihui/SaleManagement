using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class AccountBindingManager : BaseManager
    {
        public async Task<InvokedResult> CreateAccountBinding(AccountBinding accountBinding)
        {
            var isBindIng = DbContext.Set<AccountBinding>().Any(a => a.WxAccount == accountBinding.WxAccount);
            if (isBindIng)
                return InvokedResult.SucceededResult;

            DbContext.Set<AccountBinding>().AddOrUpdate(accountBinding);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<AccountBinding> GetAccountBindingAsync(string wxAccount)
        {
            Requires.NotNullOrEmpty(wxAccount, "wxAccount");

            return await DbContext.Set<AccountBinding>().FirstOrDefaultAsync(r => r.WxAccount == wxAccount);
        }

        public async Task<bool> IsBindingAsync(string userName)
        {
            Requires.NotNullOrEmpty(userName, "userName");

            return await DbContext.Set<AccountBinding>().AnyAsync(r => r.UserName == userName);
        }
    }
}
