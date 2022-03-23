using SaleManagement.Core.Models;
using System.Collections.Generic;

namespace SaleManagement.Protal.Models.SpotGoodsPattern
{
    public class SpotGoodsPatternEditViewModel
    {
        public SpotGoodsPatternEditViewModel()
        {
            SpotGoodsPatternTypeIds = new List<string>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> SpotGoodsPatternTypeIds { get; set; }

        public string SpotGoodsPatternTypeIdStr
        {
            get { return string.Join(",", SpotGoodsPatternTypeIds); }
        }

        public string FileInfoId { get; set; }

        public int ProductCategoryId { get; set; }

        public int GemCategoryId { get; set; }


        public double Price { get; set; }

        public string ReferenceData { get; set; }

        public int RowNo { get; set; }

        public List<SpotGoodType> SpotGoodTypes { get; set; }

        public IEnumerable<ProductCategory> ProductCategories { get; set; }

        public IEnumerable<GemCategory> GemCategories { get; set; }
    }
}