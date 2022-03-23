using SaleManagement.Core.Models;
using System.Collections.Generic;

namespace SaleManagement.Protal.Models.SpotGoodsPattern
{
    public class SpotGoodsPatternEditViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SpotGoodTypeId { get; set; }

        public virtual SpotGoodType SpotGoodType { get; set; }

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