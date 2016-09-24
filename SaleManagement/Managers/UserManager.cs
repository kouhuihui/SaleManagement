using Dickson.Core.ComponentModel;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Caching.Memory;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class UserManager : UserManager<SaleUser>
    {
        public UserManager() : this(new SaleUserStore())
        {
        }

        public UserManager(SaleUserStore store)
            : base(store)
        { }
    }

    public class UserManager<TUser> : Microsoft.AspNet.Identity.UserManager<TUser> where TUser : SaleUser
    {
        IMemoryCache m_MemoryCache;

        public UserManager(SaleUserStore<TUser> store)
          : base(store)
        {
            m_MemoryCache = CachingHelper.GetMemoryCache("SaleManagementUser");
        }

        public override Task<TUser> FindByEmailAsync(string email)
        {
            return Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<InvokedResult> RegisterAsync(TUser user, string password)
        {
            Requires.NotNull(user, nameof(user));
            Requires.NotNullOrEmpty(password, nameof(password));
            if (await IsRegisteredAsync(user.UserName))
                return InvokedResult.Fail("UserExists", "用户已经被注册，请勿重复注册");

            var result = await CreateAsync(user, password);

            return result.Succeeded ? InvokedResult.Ok("注册成功") : InvokedResult.Fail("CreateUser", string.Join(",", result.Errors), string.Empty);
        }

        public Task<bool> IsRegisteredAsync(string userName)
        {
            Requires.NotNullOrEmpty(userName, nameof(userName));
            return Users.AnyAsync(u => u.UserName == userName);
        }

        public SaleUser FindByIdByCache(string userId)
        {
            SaleUser user = m_MemoryCache.Get<SaleUser>(userId);
            if (user == null)
            {
                user = this.FindById(userId);
                if (user != null)
                {
                    m_MemoryCache.Set(userId, user, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(SaleManagentConstants.UI.DefaultUserCacheExpiringMinutes) });
                }
            }
            return user;
        }

        public async Task<Paging<TUser>> GetUsersAsync(int start, int take, Func<IQueryable<TUser>, IQueryable<TUser>> filter = null)
        {
            var query = Users.Where(u => u.IdentityType == IdentityType.Employee);
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).ThenByDescending(u => u.Name).Skip(start).Take(take).ToListAsync();

            return new Paging<TUser>(start, take, total, list);
        }

        public async Task<IEnumerable<TUser>> GetUserByRoleAsync(string roleCode)
        {
            return await Users.Where(u => u.IdentityType == IdentityType.Employee && u.Status == UserStatus.Normal && u.Role.Code == roleCode).ToListAsync();
        }

        public async Task<InvokedResult> UpdateUserStatus(string[] userIds, UserStatus status)
        {
            var users = await GetUsersAsync(userIds);
            foreach (var user in users)
            {
                user.Status = status;
                await base.UpdateAsync(user);
                RemoveCachedUserItem(user.Id);
            }

            return InvokedResult.SucceededResult;
        }

        public async Task<Paging<TUser>> GetCustomersAsync(int start, int take, Func<IQueryable<TUser>, IQueryable<TUser>> filter = null)
        {
            var query = Users.Where(u => u.IdentityType == IdentityType.Customer);
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).ThenByDescending(u => u.Name).Skip(start).Take(take).ToListAsync();

            return new Paging<TUser>(start, take, total, list);
        }

        public async Task<IEnumerable<TUser>> GetAllCustomersAsync()
        {
            var query = Users.Where(u => u.IdentityType == IdentityType.Customer && u.Status== UserStatus.Normal);
            return await query.OrderByDescending(u => u.Created).ThenByDescending(u => u.Name).ToListAsync();
        }

        public void RemoveCachedUserItem(string userId)
        {
            Requires.NotNullOrEmpty(userId, nameof(userId));
            m_MemoryCache.Remove(userId);
        }

        public async Task<InvokedResult> ResetPasswordAsync(string userId, string password)
        {
            Requires.NotNull(userId, nameof(userId));
            Requires.NotNull(password, nameof(password));

            var user = await FindByIdAsync(userId);
            if (user == null)
                return SaleManagentConstants.InvokedResults.UserNotFoundResult;

            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider();
            UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<TUser>(provider.Create("UserToken"))
                                                        as IUserTokenProvider<TUser, string>;

            var code = await GeneratePasswordResetTokenAsync(user.Id);
            await base.ResetPasswordAsync(user.Id, code, password);

            await UpdateAsync(user);
            return InvokedResult.SucceededResult;
        }

        private async Task<IEnumerable<TUser>> GetUsersAsync(IEnumerable<string> userIds)
        {
            if (!userIds.Any())
                return Enumerable.Empty<TUser>();

            return await Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        }
    }
}