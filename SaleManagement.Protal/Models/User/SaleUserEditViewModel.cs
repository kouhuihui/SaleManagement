using Dickson.Library.Common;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Protal.Models
{
    public class SaleUserEditViewModel
    {
        public SaleUserEditViewModel()
        {
            Telephone = string.Empty;
            Mobile = string.Empty;
            Email = string.Empty;
        }

        public SaleUserEditViewModel(SaleUser user)
        {
            Id = user.Id;
            Telephone = user.Telephone;
            Mobile = user.Mobile;
            Email = user.Email;
            UserName = user.UserName;
            Name = user.Name;
        }


        public string Id { get; set; }

        [Display(Name = "姓名"), Required(ErrorMessage = "请填写{0}"), StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        [Display(Name = "用户名"), Required(ErrorMessage = "请填写{0}"), StringLength(SaleManagentConstants.Validations.UserNameStringLength)]
        public string UserName { get; set; }

        [Display(Name = "联系电话"), StringLength(SaleManagentConstants.Validations.PhoneStringLength)]
        public string Telephone { get; set; }

        [Display(Name = "手机号"), StringLength(SaleManagentConstants.Validations.MoblieStringLength)]
        [RegularExpression(GlobalRegularPattern.SimplyMobile, ErrorMessage = "请输入正确的{0}")]
        public string Mobile { get; set; }

        [Display(Name = "邮箱"), StringLength(SaleManagentConstants.Validations.EmailStringLength)]
        [RegularExpression(GlobalRegularPattern.SimplyEmail, ErrorMessage = "请输入正确的{0}")]
        public string Email { get; set; }

        [Display(Name = "密码"), DataType(DataType.Password), Required(ErrorMessage = "请填写{0}")]
        [StringLength(SaleManagentConstants.Validations.PasswordStringLength, ErrorMessage = "{0}最长支持{1}个字符")]
        [MinLength(6, ErrorMessage = "请输入至少6位密码")]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare("Password", ErrorMessage = "两次密码输入不一致")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "角色")]
        [Required(ErrorMessage = "请选择{0}")]
        public int RoleId { get; set; }
    }
}