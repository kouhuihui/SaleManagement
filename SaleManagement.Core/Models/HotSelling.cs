using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class HotSelling
    {
        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
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

        public int ProductCategoryId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public int GemCategoryId { get; set; }

        public virtual GemCategory GemCategory { get; set; }

        /// <summary>
        /// 版号
        /// </summary>
        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string VersionNo { get; set; }

        public int RowNo { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<HotSellingAttachment> Attachments { get; set; }

    }
}
