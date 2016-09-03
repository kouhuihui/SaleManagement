using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class MatchStone
    {
        public int Id { get; set; }

        [Display(Name = "副石名称")]
        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(SaleManagentConstants.Validations.DefaultNameStringLength, ErrorMessage = "{0}最多支持{1}字符")]

        public string Name { get; set; }

        [Display(Name = "单价名称")]
        [Required(ErrorMessage = "请填写{0}")]
        [Range(typeof(double), "0.00", "20000.00",ErrorMessage ="{0}范围是0.00 ~ 19999.99")]
        public double Price { get; set; }

        [Display(Name = "工费")]
        [Required(ErrorMessage = "请填写{0}")]
        public int WorkingCost  { get; set; }

        public bool Deleted { get; set; }
    }
}
