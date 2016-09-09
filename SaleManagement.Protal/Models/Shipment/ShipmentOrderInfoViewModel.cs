using SaleManagement.Protal.Models.Order;
using System.Collections.Generic;
using System.Linq;

namespace SaleManagement.Protal.Models.Shipment
{
    public class ShipmentOrderInfoViewModel : OrderViewModelBase
    {
        public ShipmentOrderInfoViewModel():base()
        {

        }

        public ShipmentOrderInfoViewModel(Core.Models.Order order) : base(order)
        {
            OutputWaxCost = order.OutputWaxCost;
            ProductName = order.ColorForm.Name + order.GemCategory.Name + order.ProductCategory.Name;
            Weight = order.Weight;
            GoldWeight = order.GoldWeight;
            OrderSetStoneInfos = order.OrderSetStoneInfos.Select(o => new OrderSetStoneInfoViewModel
            {
                MatchStoneId = o.MatchStoneId,
                MatchStoneName = o.MathchStoneName,
                Number = o.Number,
                Weight = o.Weight,
                Price = o.Price,
                TotalAmount = o.Price * (double)o.Weight,
                SetStoneWorkingCost = o.WorkingCost
            });
        }

        public string ProductName { get; set; }

        public double Weight { get; set; }

        public double GoldWeight { get; set; }

        public double LossRate { get; set; }

        public double Hhz { get; set; }

        public double GoldPrice { get; set; }

        public double GoldAmount { get; set; }

        /// <summary>
        /// 风险费
        /// </summary>
        public double RiskFee { get; set; }

        /// <summary>
        /// 副石重
        /// </summary>
        public int SideStoneNumber { get; set; }

        /// <summary>
        /// 副石数
        /// </summary>
        public double SideStoneWeight { get; set; }

        /// <summary>
        /// 镶石工费
        /// </summary>
        public double TotalSetStoneWorkingCost { get; set; }

        /// <summary>
        /// 副石额
        /// </summary>
        public double SideStoneTotalAmount { get; set; }

        /// <summary>
        /// 基本费用
        /// </summary>
        public double BasicCost { get; set; }

        public double OutputWaxCost { get; set; }

        public double OtherCost { get; set; }

        /// <summary>
        /// 总额
        /// </summary>
        public double TotalAmount { get; set; }

        public IEnumerable<OrderSetStoneInfoViewModel> OrderSetStoneInfos { get; set; }
    }
}