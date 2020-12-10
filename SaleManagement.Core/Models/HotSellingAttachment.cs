using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class HotSellingAttachment
    {
        public HotSellingAttachment()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }

        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string HotSellingId { get; set; }

        public virtual HotSelling HotSelling { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string FileInfoId { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [Required]
        public int FileType { get; set; }

        public DateTime Created { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string CreatorId { get; set; }
    }
}
