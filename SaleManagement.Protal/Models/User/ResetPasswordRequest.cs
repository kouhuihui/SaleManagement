using SaleManagement.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.User
{
    public class ResetPasswordRequest
    {
        [DataType(DataType.Password), Compare("Password", ErrorMessage = "确认密码须和新密码一致。")]
        public string ConfirmNewPassword { get; set; }

        [StringLength(SaleManagentConstants.Validations.PasswordStringLength, ErrorMessage = "{0}最长支持{1}个字符")]
        [MinLength(6, ErrorMessage = "请输入至少6位密码")]
        [Display(Name = "新密码"), DataType(DataType.Password), Required(ErrorMessage = "请填写{0}")]
        public string Password { get; set; }


        [Required]
        public string UserId { get; set; }
    }
}