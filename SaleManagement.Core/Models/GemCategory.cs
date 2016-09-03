﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class GemCategory
    {
        public int Id { get; set; }

        [Display(Name = "宝石名称")]
        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(SaleManagentConstants.Validations.DefaultNameStringLength, ErrorMessage = "{0}最多支持{1}字符")]

        public string Name { get; set; }

        public bool Deleted { get; set; }
    }
}
