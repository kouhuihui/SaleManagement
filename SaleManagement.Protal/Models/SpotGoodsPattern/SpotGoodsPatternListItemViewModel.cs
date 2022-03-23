namespace SaleManagement.Protal.Models
{
    public class SpotGoodsPatternListItemViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public string FileInfoId { get; set; }

        public string Url { get; set; }

        public int ProductCategoryId { get; set; }

        public string ProductCategoryName { get; set; }


        public int GemCategoryId { get; set; }

        public string GemCategoryName { get; set; }

        public double Price { get; set; }

        public string ReferenceData { get; set; }
    }
}