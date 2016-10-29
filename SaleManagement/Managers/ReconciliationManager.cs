using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public class ReconciliationManager : BaseManager
    {
        public ReconciliationManager()
        {

        }

        public ReconciliationManager(SaleUser user) : base(user)
        {

        }

        public async Task<Reconciliation> GetReconciliationAsync(int id)
        {
            return await DbContext.Set<Reconciliation>().FirstOrDefaultAsync(r => r.Id == id && r.CompanyId == User.CompanyId);
        }

        public async Task<InvokedResult> CreateAsync(Reconciliation reconciliation)
        {
            DbContext.Set<Reconciliation>().AddOrUpdate(reconciliation);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> DeleteReconciliationAsync(string shipmentOrderId)
        {
            var remark = shipmentOrderId + "出货";
            var reconciliation = DbContext.Set<Reconciliation>().FirstOrDefault(r => r.Remark == remark);
            if (reconciliation == null)
                return InvokedResult.Fail("404", "不存在该出货单的对账记录");

            DbContext.Set<Reconciliation>().Remove(reconciliation);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> DeleteReconciliationAsync(Reconciliation reconciliation)
        {
            DbContext.Set<Reconciliation>().Remove(reconciliation);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<Paging<Reconciliation>> GetReconciliationsAsync(int start, int take, Func<IQueryable<Reconciliation>, IQueryable<Reconciliation>> filter = null)
        {
            var query = DbContext.Set<Reconciliation>().Where(o => o.CompanyId == User.CompanyId);
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).Skip(start).Take(take).ToListAsync();

            return new Paging<Reconciliation>(start, take, total, list);
        }

        public async Task<IEnumerable<Reconciliation>> GetReconciliationsAsync(Func<IQueryable<Reconciliation>, IQueryable<Reconciliation>> filter = null)
        {
            var query = DbContext.Set<Reconciliation>().Where(o => o.CompanyId == User.CompanyId);
            if (filter != null)
            {
                query = filter(query);
            }
            var list = await query.OrderByDescending(u => u.Created).ToListAsync();

            return list;
        }

        public async Task<IEnumerable<Reconciliation>> GetCustomerReconciliationsAsync(Func<IQueryable<Reconciliation>, IQueryable<Reconciliation>> filter = null)
        {
            var query = DbContext.Set<Reconciliation>().Where(o => o.CustomerId == User.Id);
            if (filter != null)
            {
                query = filter(query);
            }
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(u => u.Created).ToListAsync();

            return list;
        }

        public async Task<IEnumerable<AccountStatistic>> GetAccountStatisticsAsync(ReportQueryBaseDto reportQuery)
        {
            var query = DbContext.Set<Reconciliation>().Where(o => o.CompanyId == User.CompanyId);
            if (!string.IsNullOrEmpty(reportQuery.CustomerId))
            {
                query = query.Where(o => o.CustomerId == reportQuery.CustomerId);
            }
            var AccountStatistics = await query.GroupBy(r => r.CustomerId).Select(a => new
            {
                CustomerId = a.Key,
                SurplusArrearage = a.Sum(r => r.Type == ReconciliationType.Payment ? -r.Amount : r.Amount),
            }).ToListAsync();

            if (reportQuery.StatisticStartDate.HasValue)
            {
                query = query.Where(o => o.Created > reportQuery.StatisticStartDate.Value);
            }
            if (reportQuery.StatisticEndDate.HasValue)
            {
                var endDate = reportQuery.StatisticEndDate.Value.AddDays(1);
                query = query.Where(o => o.Created < endDate);
            }

            var AccountStatisticsInQuery = await query.Where(r => r.Type == ReconciliationType.Payment).GroupBy(r => r.CustomerId).Select(a => new
            {
                CustomerId = a.Key,
                Payment = a.Sum(r => r.Amount)
            }).ToListAsync();

            var customers = await new UserManager().GetAllCustomersAsync();
            var statistic = (from a in AccountStatistics
                             join b in AccountStatisticsInQuery
                            on a.CustomerId equals b.CustomerId
                            into temp
                             from ur in temp.DefaultIfEmpty()
                             join c in customers
                             on a.CustomerId equals c.Id
                             select new AccountStatistic
                             {
                                 CustomerId = a.CustomerId,
                                 CustomerName = c.Name,
                                 PaymentInQuery = ur?.Payment ?? 0,
                                 SurplusArrearage = Math.Round(a.SurplusArrearage, 2)
                             }).OrderByDescending(c => c.SurplusArrearage).ToList();
            var totalPaymentInQuery = Math.Round(statistic.Sum(r => r.PaymentInQuery), 2);
            var totalSurplusArrearage = Math.Round(statistic.Sum(r => r.SurplusArrearage), 2);
            statistic.Add(new AccountStatistic { CustomerName = "总计", PaymentInQuery = totalPaymentInQuery, SurplusArrearage = totalSurplusArrearage });

            return statistic;
        }

        public async Task<double> GetTotalSurplusArrearageAsync(string customerId)
        {
            var Reconciliations = DbContext.Set<Reconciliation>().Where(o => o.CompanyId == User.CompanyId && o.CustomerId == customerId);
            if (!Reconciliations.Any())
                return 0;

            var total = await Reconciliations.SumAsync(o => o.Type == ReconciliationType.Payment ? -o.Amount : o.Amount);
            return Math.Round(total, 2);
        }
    }
}
