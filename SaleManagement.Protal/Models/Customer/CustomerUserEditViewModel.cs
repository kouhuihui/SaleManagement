using SaleManagement.Core;
using SaleManagement.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Protal.Models.Customer
{
    public class CustomerUserEditViewModel : SaleUserEditViewModel
    {
        public CustomerUserEditViewModel() : base()
        {
            StoneSetterDiscountRate = 100;
            SideStoneDiscountRate = 100;
            PriceOfWorkDiscountRate = 100;
        }

        public CustomerUserEditViewModel(SaleUser user) : base(user)
        {
        }

        [Display(Name = "镶石折扣率")]
        [Range(1, 100, ErrorMessage = "请输入1~100之间数字")]
        public int StoneSetterDiscountRate { get; set; }

        [Display(Name = "副石折扣率")]
        [Range(1, 100, ErrorMessage = "请输入1~100之间数字")]
        public int SideStoneDiscountRate { get; set; }

        [Display(Name = "工费折扣率")]
        [Range(1, 100, ErrorMessage = "请输入1~100之间数字")]
        public int PriceOfWorkDiscountRate { get; set; }

        [Display(Name = "18K耗损比")]
        [Range(1, 20, ErrorMessage = "请输入1~20之间数字")]
        public int Loss18KRate { get; set; }

        [Display(Name = "Pt耗损比")]
        [Range(1, 20, ErrorMessage = "请输入1~20之间数字")]
        public int LossPtRate { get; set; }

        [Display(Name = "地址")]
        [StringLength(SaleManagentConstants.Validations.DefaultStringLength,ErrorMessage ="{0}最长为{1}个字符")]
        public string Address { get; set; }
    }
}