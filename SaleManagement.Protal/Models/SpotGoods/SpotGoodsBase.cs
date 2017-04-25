using SaleManagement.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Protal.Models.SpotGoods
{
    public class SpotGoodsBase
    {
        public SpotGoodsBase()
        {
        }

        public string Id { get; set; }

        public string SpotGoodsPatternId { get; set; }

        public string SpotGoodsPatternName { get; set; }

        public SpotGoodsType SpotGoodsType { get; set; }

        [Display(Name = "成色")]
        [Required(ErrorMessage = "请选择{0}")]
        public int ColorFormId { get; set; }

        [Display(Name = "手寸")]
        [Required(ErrorMessage = "请选择{0}")]
        [Range(7, 18, ErrorMessage = "{0}的范围为7-18")]
        public int HandSize { get; set; }

        [Display(Name = "主石")]
        [Required(ErrorMessage = "请选择{0}大小")]
        public string MainStone { get; set; }

        [Display(Name = "总重")]
        [RegularExpression("^[0-9]+(.[0-9]{1,3})?$", ErrorMessage = "请输入三位小数的数字")]
        [Required(ErrorMessage = "请输入{0}")]
        public double Weight { get; set; }

        [Display(Name = "净金重")]
        [RegularExpression("^[0-9]+(.[0-9]{1,3})?$", ErrorMessage = "请输入三位小数的数字")]
        [Required(ErrorMessage = "请输入{0}")]
        public double GoldWeight { get; set; }

        public double MosaicCost { get; set; }

        public bool IsMosaic { get; set; }

        public string CreatorId { get; set; }

        public bool IsLock { get; set; }

        [Display(Name = "总额")]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入二位小数的数字")]
        public double Price { get; set; }

        public double BasicCost { get; set; }

        [Display(Name = "18K耗损比")]
        [Range(1, 20, ErrorMessage = "请输入1~20之间数字")]
        public int Loss18KRate { get; set; }

        public int Status { get; set; }
    }
}