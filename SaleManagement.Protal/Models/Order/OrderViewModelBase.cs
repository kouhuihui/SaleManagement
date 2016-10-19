using Dickson.Core.Common.Extensions;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Protal.Models.Order
{
    public class OrderViewModelBase
    {
        public OrderViewModelBase()
        {
            MainStoneSize = 0;
            Number = 1;
            Certificate = "0";
            RabbetRequirement = SaleManagement.Core.Models.RabbetRequirement.Normal;
            StoneDescribe = SaleManagement.Core.Models.StoneDescribe.Normal;
            GoldWeightRequirement = SaleManagement.Core.Models.GoldWeightRequirement.Normal;
            SideStoneRequiredment = SaleManagement.Core.Models.SideStoneRequiredment.Normal;
            RadianRequirement = SaleManagement.Core.Models.RadianRequirement.Normal;
            HasOldMaterial = false;
        }

        public OrderViewModelBase(Core.Models.Order order)
        {
            Id = order.Id;
            MainStoneSize = order.MainStoneSize;
            MainStoneNumber = order.MainStoneNumber;
            Number = order.Number;
            Certificate = order.Certificate;
            RabbetRequirement = order.RabbetRequirement;
            StoneDescribe = order.StoneDescribe;
            GoldWeightRequirement = order.GoldWeightRequirement;
            SideStoneRequiredment = order.SideStoneRequiredment;
            RadianRequirement = order.RadianRequirement;
            HasOldMaterial = order.HasOldMaterial;
            RangSize = GetRang(order);
            WordsPrinted = order.WordsPrinted;
            Remark = order.Remark;
            ProductCategoryId = order.ProductCategoryId;
            ColorFormId = order.ColorFormId;
            GemCategoryId = order.GemCategoryId;
            CustomerId = order.CustomerId;
            HandSize = order.HandSize;
            MinChainLength = order.MinChainLength;
            MaxChainLength = order.MaxChainLength;
            DeliveryDate = order.DeliveryDate?.ToString(SaleManagentConstants.UI.DateStringFormat) ?? "";
            CustomerName = order.Customer.Name;
            Created = order.Created.ToString(SaleManagentConstants.UI.DateStringFormat);
            ColorFormName = order.ColorForm?.Name;
            GemCategoryName = order.GemCategory?.Name;
            ProductCategoryName = order.ProductCategory?.Name;
            OutputWaxCost = order.OutputWaxCost;
            ModuleTypeName = order.ModuleType.GetDisplayName();
        }

        public string Id { get; set; }

        [Display(Name = "手寸")]
        [Required(AllowEmptyStrings = true)]
        [Range(0, 99, ErrorMessage = "{0}的范围是0.1-99")]
        public double HandSize { get; set; }

        [Display(Name = "链长")]
        [Required(AllowEmptyStrings = true)]
        [Range(0, 99, ErrorMessage = "{0}的范围是0.1-99")]
        public double MinChainLength { get; set; }

        [Display(Name = "链长")]
        [Required(AllowEmptyStrings = true)]
        [Range(0, 99, ErrorMessage = "{0}的范围是0.1-99")]
        public double MaxChainLength { get; set; }

        [Display(Name = "件数")]
        [Required(ErrorMessage = "请输入{0}")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "请输入整数")]
        [Range(1, 1000, ErrorMessage = "{0}范围是1-1000")]
        public int Number { get; set; }

        [Display(Name = "主石数量")]
        [Required(ErrorMessage = "请输入{0}")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "请输入整数")]
        [Range(0, 1000, ErrorMessage = "{0}范围是0-1000")]
        public int MainStoneNumber { get; set; }

        [Display(Name = "主石大小")]
        [RegularExpression("^[0-9]+(.[0-9]{1,3})?$", ErrorMessage = "请输入三位小数的数字")]
        [Required(ErrorMessage = "请输入{0}")]
        public double MainStoneSize { get; set; }

        [Display(Name = "证书号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(10, ErrorMessage = "{0}不能超过{1}个字符")]
        public string Certificate { get; set; }

        [StringLength(SaleManagentConstants.Validations.GeneralShorterStringLength, ErrorMessage = "{0}不能超过{1}个字符")]
        public string WordsPrinted { get; set; }

        public bool HasOldMaterial { get; set; }

        [Display(Name = "客户留言")]
        [StringLength(SaleManagentConstants.Validations.DefaultStringLength, ErrorMessage = "{0}不能超过{1}个字符")]
        public string Remark { get; set; }

        [Display(Name = "品类")]
        [Required(ErrorMessage = "请选择{0}")]
        public int ProductCategoryId { get; set; }

        [Display(Name = "成色")]
        [Required(ErrorMessage = "请选择{0}")]
        public int ColorFormId { get; set; }

        [Display(Name = "宝石品类")]
        [Required(ErrorMessage = "请选择{0}")]
        public int GemCategoryId { get; set; }

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

        [Display(Name = "客户")]
        [Required(ErrorMessage = "请选择{0}")]
        public string CustomerId { get; set; }

        public string DeliveryDate { get; set; }

        public string RangSize { get; set; }

        public string CustomerName { get; set; }

        public string Created { get; set; }

        public string ColorFormName { get; set; }

        public string GemCategoryName { get; set; }

        public string ProductCategoryName { get; set; }

        public double OutputWaxCost { get; set; }

        public string ModuleTypeName { get; set; }

        string GetRang(SaleManagement.Core.Models.Order order)
        {
            switch (order.ProductCategory.Name)
            {
                case "女戒":
                case "男戒":
                case "手镯":
                    return order.HandSize.ToString();
                case "吊坠":
                case "手链":
                    return order.MinChainLength + "-" + order.MaxChainLength;
                default:
                    return "";
            }
        }
    }
}