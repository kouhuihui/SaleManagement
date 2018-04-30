using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
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
    }
}
