using Dickson.Core.Common.Extensions;
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
    public class OrderMainStoneInfoManager : BaseManager
    {
        public OrderMainStoneInfoManager()
        {

        }

        public OrderMainStoneInfoManager(SaleUser user) : base(user)
        {

        }

        public async Task<InvokedResult> CreateOrderMainStoneInfoAsync(OrderMainStoneInfo info, string[] attachmentIds)
        {
            DbContext.Set<OrderMainStoneInfo>().AddOrUpdate(info);

            if (attachmentIds.Any())
            {
                DbContext.Set<OrderMainStoneAttachment>()
                    .AddRange(attachmentIds.Select(r => new OrderMainStoneAttachment
                    {
                        Id = Guid.NewGuid().ToString(),
                        FileInfoId = r,
                        Created = DateTime.Now,
                        CreatorId = User.Id,
                        OrderMainStoneInfoId = info.Id

                    }));

            }

            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<InvokedResult> DeleteOrderSetStoneAsync(int id)
        {
            var orderMainStoneInfo = DbContext.Set<OrderMainStoneInfo>().FirstOrDefault(r => r.Id == id);
            if (orderMainStoneInfo == null)
                return InvokedResult.Fail("404", "主石不存在");

            DbContext.Set<OrderMainStoneInfo>().Remove(orderMainStoneInfo);
            await DbContext.SaveChangesAsync();
            return InvokedResult.SucceededResult;
        }

        public async Task<IEnumerable<OrderMainStoneAttachment>> GetOrderMainStoneAttachments(int id)
        {
            return await DbContext.Set<OrderMainStoneAttachment>().Where(r => r.OrderMainStoneInfoId == id).ToListAsync();
        }

        public async Task<IEnumerable<OrderMainStoneAttachment>> GetOrderMainStoneAttachmentsByOrderId(string orderId)
        {
            var result = await (from a in DbContext.Set<OrderMainStoneInfo>()
                                join b in DbContext.Set<OrderMainStoneAttachment>() on a.Id equals b.OrderMainStoneInfoId
                                where a.OrderId == orderId
                                select b).ToListAsync();


            return result;
        }

        public async Task<IEnumerable<OrderMainStoneStatistic>> GetOrderMainStoneStatisticsAsync(
            ReportQueryBaseDto reportQuery)
        {

            var query = DbContext.Set<OrderMainStoneInfo>().AsQueryable();
            if (!string.IsNullOrEmpty(reportQuery.CustomerId))
            {
                query = query.Where(a => a.Order.CustomerId == reportQuery.CustomerId);
            }

            if (reportQuery.StatisticStartDate.HasValue)
            {
                query = query.Where(t => t.Created > reportQuery.StatisticStartDate.Value);
            }
            if (reportQuery.StatisticEndDate.HasValue)
            {
                var endDate = reportQuery.StatisticEndDate.Value.AddDays(1);
                query = query.Where(t => t.Created < endDate);
            }

            if (!string.IsNullOrEmpty(reportQuery.OrderId))
            {
                query = query.Where(a => a.OrderId.Contains(reportQuery.OrderId));
            }

            return (await query.OrderBy(t => t.Created).ToListAsync()).Select(t => new OrderMainStoneStatistic
            {
                OrderId = t.OrderId,
                CustomerName = t.Order.Customer.Name,
                Created = t.Created.ToShortDateString(),
                MainStoneName = t.MainStone.Name,
                Risk = t.MainStone.RiskType.GetDisplayName(),
                MainStoneWeight = t.Weight
            });
        }
    }
}
