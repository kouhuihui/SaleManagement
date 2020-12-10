using SaleManagement.Core;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Protal.Models.HotSelling
{
    public class HotSellingBase
    {
        public string Id { get; set; }


        /// <summary>
        /// 品名
        /// </summary>
        [Required, StringLength(SaleManagentConstants.Validations.DefaultNameStringLength)]
        public string Name { get; set; }


        /// <summary>
        /// 参考价
        /// </summary>
        public double ReferencePrice { get; set; }

        /// <summary>
        /// 参考数据
        /// </summary>
        public string ReferenceData { get; set; }

        /// <summary>
        /// 品类
        /// </summary>
        [Required]
        public int ProductCategoryId { get; set; }

        /// <summary>
        /// 宝石品类
        /// </summary>
        [Required]
        public int GemCategoryId { get; set; }

        /// <summary>
        /// 版号
        /// </summary> 
        public string VersionNo { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Required]
        [Range(typeof(int), "0", "10000", ErrorMessage = "{0}范围是0 ~ 10000")]
        public int RowNo { get; set; }

    }
}