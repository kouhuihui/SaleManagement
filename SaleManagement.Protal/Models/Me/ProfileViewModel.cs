using Dickson.Library.Common;
using SaleManagement.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Me
{
    public class ProfileViewModel
    {
        [Display(Name = "姓名"), Required(ErrorMessage = "请填写{0}"), StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }

        [Display(Name = "联系电话"), StringLength(SaleManagentConstants.Validations.PhoneStringLength)]
        public string Telephone { get; set; }

        [Display(Name = "手机号"), StringLength(SaleManagentConstants.Validations.MoblieStringLength)]
        [RegularExpression(GlobalRegularPattern.SimplyMobile, ErrorMessage = "请输入正确的{0}")]
        public string Mobile { get; set; }

        [Display(Name = "邮箱"), StringLength(SaleManagentConstants.Validations.EmailStringLength)]
        [RegularExpression(GlobalRegularPattern.SimplyEmail, ErrorMessage = "请输入正确的{0}")]
        public string Email { get; set; }
    }
}