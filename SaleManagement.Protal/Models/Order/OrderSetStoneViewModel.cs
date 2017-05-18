using SaleManagement.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace SaleManagement.Protal.Models.Order
{
    public class OrderSetStoneViewModel : OrderViewModelBase
    {
        public OrderSetStoneViewModel()
        {

        }

        public OrderSetStoneViewModel(Core.Models.Order order) : base(order)
        {
            OrderSetStoneInfos = order.OrderSetStoneInfos.Select(o => new OrderSetStoneInfoViewModel
            {
                Id = o.Id,
                MatchStoneId = o.MatchStoneId,
                MatchStoneName = o.MathchStoneName,
                Number = o.Number,
                Weight = o.Weight

            });
        }

        public IEnumerable<OrderSetStoneInfoViewModel> OrderSetStoneInfos { get; set; }

        public RiskType RiskType { get; set; }
    }
}