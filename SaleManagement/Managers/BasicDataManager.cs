﻿using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using SaleManagement.Core.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class BasicDataManager : BaseManager
    {
        public BasicDataManager()
        {
        }

        public BasicDataManager(SaleUser user) : base(user)
        {
        }

        public async Task<IEnumerable<ProductCategory>> GetProductCategoriesAsync()
        {
            return await DbContext.Set<ProductCategory>().Where(r => !r.Deleted).OrderBy(d => d.Id).ToListAsync();
        }

        public async Task<ProductCategory> GetProductCategoryAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            return await DbContext.Set<ProductCategory>().FirstOrDefaultAsync(r => !r.Deleted && r.Id == id);
        }

        public async Task<InvokedResult> DeleteProductCategoryAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            var dbSet = DbContext.Set<ProductCategory>();
            var productCategory = await dbSet.FirstOrDefaultAsync(c => c.Id == id);
            if (productCategory == null)
                return InvokedResult.Fail("productCategoryNoExists", "数据不存在");

            productCategory.Deleted = true;
            dbSet.AddOrUpdate(productCategory);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> SaveProductCategoryAsync(ProductCategory productCategory)
        {
            DbContext.Set<ProductCategory>().AddOrUpdate(productCategory);
            await DbContext.SaveChangesAsync();

            return InvokedResult.SucceededResult;
        }

        public async Task<IEnumerable<GemCategory>> GetGemCategoriesAsync()
        {
            return await DbContext.Set<GemCategory>().Where(r => !r.Deleted).OrderBy(d => d.Id).ToListAsync();
        }

        public async Task<GemCategory> GetGemCategoryAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            return await DbContext.Set<GemCategory>().FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        }

        public async Task<InvokedResult> DeleteGemCategoryAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            var dbSet = DbContext.Set<GemCategory>();
            var gemCategory = await dbSet.FirstOrDefaultAsync(c => c.Id == id);
            if (gemCategory == null)
                return InvokedResult.Fail("gemCategoryNoExists", "数据不存在");

            gemCategory.Deleted = true;
            dbSet.AddOrUpdate(gemCategory);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> SaveGemCategoryAsync(GemCategory gemCategory)
        {
            DbContext.Set<GemCategory>().AddOrUpdate(gemCategory);
            await DbContext.SaveChangesAsync();

            return InvokedResult.SucceededResult;
        }

        public async Task<IEnumerable<ColorForm>> GetColorFormsAsync()
        {
            return await DbContext.Set<ColorForm>().Where(r => !r.Deleted).OrderByDescending(d => d.Id).ToListAsync();
        }

        public async Task<ColorForm> GetColorFormAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            return await DbContext.Set<ColorForm>().FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        }

        public async Task<InvokedResult> SaveColorFormAsync(ColorForm colorForm)
        {
            DbContext.Set<ColorForm>().AddOrUpdate(colorForm);
            await DbContext.SaveChangesAsync();

            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> DeleteColorFormAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            var dbSet = DbContext.Set<ColorForm>();
            var colorForm = await dbSet.FirstOrDefaultAsync(c => c.Id == id);
            if (colorForm == null)
                return InvokedResult.Fail("ColorFormNoExists", "数据不存在");

            colorForm.Deleted = true;
            dbSet.AddOrUpdate(colorForm);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<IEnumerable<MatchStone>> GetMatchStonesAsync()
        {
            return await DbContext.Set<MatchStone>().Where(m => !m.Deleted).OrderBy(d => d.Id).ToListAsync();
        }

        public async Task<MatchStone> GetMatchStoneAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            return await DbContext.Set<MatchStone>().FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        }

        public async Task<InvokedResult> DeleteMatchStoneAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            var dbSet = DbContext.Set<MatchStone>();
            var matchStone = await dbSet.FirstOrDefaultAsync(c => c.Id == id);
            if (matchStone == null)
                return InvokedResult.Fail("MatchStoneAsyncNoExists", "数据不存在");

            matchStone.Deleted = true;
            dbSet.AddOrUpdate(matchStone);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> SaveMatchStoneAsync(MatchStone matchStone)
        {
            DbContext.Set<MatchStone>().AddOrUpdate(matchStone);
            await DbContext.SaveChangesAsync();

            return InvokedResult.SucceededResult;
        }

        public async Task<ShippingScheduleSetting> GetShippingScheduleSettingAsync()
        {
            return await DbContext.Set<ShippingScheduleSetting>().FirstOrDefaultAsync(r => r.CompanyId == User.CompanyId);
        }

        public async Task<InvokedResult> SaveShippingScheduleSettingAsync(ShippingScheduleSetting shippingScheduleSetting)
        {
            shippingScheduleSetting.CompanyId = User.CompanyId;
            shippingScheduleSetting.UserId = User.Id;
            DbContext.Set<ShippingScheduleSetting>().AddOrUpdate(shippingScheduleSetting);
            await DbContext.SaveChangesAsync();

            return InvokedResult.SucceededResult;
        }

        public async Task<IEnumerable<MainStone>> GetMainStonesync()
        {
            return await DbContext.Set<MainStone>().Where(r => !r.Deleted).OrderByDescending(d => d.Id).ToListAsync();
        }

        public async Task<MainStone> GetMainStoneAsync(int id)
        {
            return await DbContext.Set<MainStone>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<InvokedResult> DeleteMainStoneAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            var dbSet = DbContext.Set<MainStone>();
            var mainStone = await dbSet.FirstOrDefaultAsync(c => c.Id == id);
            if (mainStone == null)
                return InvokedResult.Fail("mainStoneNoExists", "数据不存在");

            mainStone.Deleted = true;
            dbSet.AddOrUpdate(mainStone);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> SaveMainStoneAsync(MainStone mainStone)
        {
            DbContext.Set<MainStone>().AddOrUpdate(mainStone);
            await DbContext.SaveChangesAsync();

            return InvokedResult.SucceededResult;
        }
    }
}
