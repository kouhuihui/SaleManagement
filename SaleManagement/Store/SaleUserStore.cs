using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace SaleManagement.Store
{
    public class SaleUserStore : SaleUserStore<SaleUser>
    {
        public SaleUserStore()
        {
        }

        public SaleUserStore(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public class SaleUserStore<TUser> : SaleManagementStore, IUserStore<TUser>,
     IUserPasswordStore<TUser>,
     IUserEmailStore<TUser>,
     IUserSecurityStampStore<TUser>,
     IUserLockoutStore<TUser, string>,
     IQueryableUserStore<TUser, string> where TUser : SaleUser
    {
        IQueryable<TUser> m_Users;

        public SaleUserStore()
        {
        }

        public SaleUserStore(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task CreateAsync(TUser user)
        {
            DbContext.Set<TUser>().Add(user);
            return DbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(TUser user)
        {
            DbContext.Set<TUser>().Remove(user);
            return DbContext.SaveChangesAsync();
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            return DbContext.Set<TUser>().SingleOrDefaultAsync(u => u.Id == userId);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return DbContext.Set<TUser>().SingleOrDefaultAsync(u => u.UserName == userName);
        }

        public Task UpdateAsync(TUser user)
        {
            ((DbContext)DbContext).Entry(user).State = EntityState.Modified;
            return DbContext.SaveChangesAsync();
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            return Task.Run(() =>
            {
                user.PasswordHash = passwordHash;//加密后
            });
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash == null);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            return Task.Run(() =>
            {
                user.Email = email;
            });
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            return Task.Run(() =>
            {
                user.EmailConfirmed = confirmed;
            });
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            return DbContext.Set<TUser>().SingleOrDefaultAsync(u => u.Email == email);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            return Task.Run(() =>
            {
                user.SecurityStamp = stamp;
            });
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEndDateUtc != default(DateTime)
                ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc, DateTimeKind.Utc))
                : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            return Task.Run(() =>
            {
                user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? default(DateTime) : lockoutEnd.UtcDateTime;
            });
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            return Task.Run(() =>
            {
                user.AccessFailedCount = 0;
            });
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(true); //默认所有账户都支持锁定。
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            return Task.Run(() =>
            {
            });
        }

        public IQueryable<TUser> Users
        {
            get
            {
                if (m_Users == null)
                {
                    m_Users = DbContext.Set<TUser>().AsQueryable();
                }

                return m_Users;
            }
        }
    }
}
