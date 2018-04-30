using SaleManagement.Core;
using SaleManagement.Protal.Models.Order;
using System.Collections.Generic;
using System.Linq;

namespace SaleManagement.Protal.Models
{
    public class OrderSetMainStoneViewModel : OrderViewModelBase
    {
        public OrderSetMainStoneViewModel()
        {

        }

        public OrderSetMainStoneViewModel(Core.Models.Order order) : base(order)
        {
            OrderMainStoneInfos = order.OrderMainStoneInfos.Select(o => new OrderMainStoneInfoViewModel
            {
                Id = o.Id,
                MainStoneName = o.MainStone.Name,
                RiskType = o.MainStone.RiskType,
                Weight = o.Weight,
                Created = o.Created.ToString(SaleManagentConstants.UI.DateStringFormat)
            });
        }

        public IEnumerable<OrderMainStoneInfoViewModel> OrderMainStoneInfos { get; set; }
    }
}