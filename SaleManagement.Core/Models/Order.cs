using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class Order
    {
        public Order()
        {
            Number = 0;
            StoneDescribe = StoneDescribe.Normal;
            RabbetRequirement = RabbetRequirement.Normal;
            GoldWeightRequirement = GoldWeightRequirement.Normal;
            SideStoneRequiredment = SideStoneRequiredment.Normal;
            RadianRequirement = RadianRequirement.Normal;
            Created = DateTime.Now;
            Updated = DateTime.Now;
            OrderStatus = OrderStatus.UnConfirmed;
            OrderRushStatus = OrderRushStatus.Normal;
        }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        public int ColorFormId { get; set; }

        public virtual ColorForm ColorForm { get; set; }

        public int ProductCategoryId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public int GemCategoryId { get; set; }

        public virtual GemCategory GemCategory { get; set; }

        public double HandSize { get; set; }

        public double MinChainLength { get; set; }

        public double MaxChainLength { get; set; }

        public int Number { get; set; }

        /// <summary>
        /// 主石数
        /// </summary>
        public int MainStoneNumber { get; set; }

        /// <summary>
        /// 主石（客来石重）
        /// </summary>
        public double MainStoneSize { get; set; }

        [StringLength(10)]
        public string Certificate { get; set; }

        /// <summary>
        /// 字印
        /// </summary>
        [StringLength(SaleManagentConstants.Validations.GeneralShorterStringLength)]
        public string WordsPrinted { get; set; }

        /// <summary>
        /// 主镶口要求
        /// </summary>
        public RabbetRequirement RabbetRequirement { get; set; }

        /// <summary>
        /// 客来石要求
        /// </summary>
        public StoneDescribe StoneDescribe { get; set; }

        /// <summary>
        /// 金石要求
        /// </summary>
        public GoldWeightRequirement GoldWeightRequirement { get; set; }

        /// <summary>
        /// 副石要求
        /// </summary>
        public SideStoneRequiredment SideStoneRequiredment { get; set; }

        public RadianRequirement RadianRequirement { get; set; }

        public bool HasOldMaterial { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultStringLength)]
        public String Remark { get; set; }

        public DateTime Created { get; set; }

        [Required]
        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CreatorId { get; set; }

        [Required]
        [StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string CreatorName { get; set; }

        public DateTime Updated { get; set; }

        public int ComplayId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        public virtual SaleUser Customer { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public ModuleType ModuleType { get; set; }

        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CurrentUserId { get; set; }

        public virtual ICollection<OrderAttachment> Attachments { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public virtual ICollection<OrderSetStoneInfo> OrderSetStoneInfos { get; set; }

        /// <summary>
        /// 出蜡倒模费用
        /// </summary>
        public double OutputWaxCost { get; set; }

        public double Weight { get; set; }

        public double GoldWeight { get; set; }

        public OrderRushStatus OrderRushStatus { get; set; }
    }
}
