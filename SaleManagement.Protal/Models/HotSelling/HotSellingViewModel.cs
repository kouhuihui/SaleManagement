using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace SaleManagement.Protal.Models.HotSelling
{
    public class HotSellingViewModel : HotSellingBase
    {

        public ProductCategory ProductCategory { get; set; }

        public GemCategory GemCategory { get; set; }

        public HotSellingViewModel()
        {
            Attachments = new List<AttachmentItem>();
            ParamAttachments = new List<AttachmentItem>();
        }

        public HotSellingViewModel(SaleManagement.Core.Models.HotSelling hotSelling)
        {
            Id = hotSelling.Id;
            VersionNo = hotSelling.VersionNo;
            ProductCategory = hotSelling.ProductCategory;
            ProductCategoryId = hotSelling.ProductCategory.Id;
            GemCategory = hotSelling.GemCategory;
            GemCategoryId = hotSelling.GemCategory.Id;
            RowNo = hotSelling.RowNo;
            Name = hotSelling.Name;
            ReferenceData = hotSelling.ReferenceData;
            ReferencePrice = hotSelling.ReferencePrice;
            Attachments =
                    hotSelling.Attachments.Where(t => t.FileType == 0).OrderByDescending(a => a.Created).Select(a => new AttachmentItem
                    {
                        Id = a.FileInfoId,
                        Url = "/Attachment/" + a.FileInfoId + "/Thumbnail"
                    }).ToList();

            ParamAttachments =
                    hotSelling.Attachments.Where(t => t.FileType == 1).OrderByDescending(a => a.Created).Select(a => new AttachmentItem
                    {
                        Id = a.FileInfoId,
                        Url = "/Attachment/" + a.FileInfoId + "/Thumbnail"
                    }).ToList();
        }

        public IEnumerable<ProductCategory> ProductCategories { get; set; }

        public IEnumerable<GemCategory> GemCategories { get; set; }

        public IEnumerable<AttachmentItem> Attachments { get; set; }

        public IEnumerable<AttachmentItem> ParamAttachments { get; set; }
    }
}