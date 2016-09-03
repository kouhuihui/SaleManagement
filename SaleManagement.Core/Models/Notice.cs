using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class Notice
    {
        public int Id { get; set; }

        [Display(Name = "内容")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(SaleManagentConstants.Validations.DefaultStringLength, ErrorMessage = "{0}最长为{1}个字符")]
        public string Content { get; set; }

        public DateTime Created { get; set; }

        public string CreatorId { get; set; }

        public int CompanyId { get; set; }
    }
}
