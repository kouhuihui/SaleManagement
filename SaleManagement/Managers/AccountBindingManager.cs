﻿using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
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

        public async Task<AccountBinding> GetAccountBindingByCustomerId(string customerId)
        {
            Requires.NotNullOrEmpty(customerId, "customerId");

            return await (from a in DbContext.Set<AccountBinding>()
                          join b in DbContext.Set<SaleUser>() on a.UserName equals b.UserName
                          where b.Id == customerId
                          select a).FirstOrDefaultAsync();
        }
    }
}
