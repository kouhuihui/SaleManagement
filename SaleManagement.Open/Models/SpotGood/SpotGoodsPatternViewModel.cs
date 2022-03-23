using SaleManagement.Core;
using SaleManagement.Core.Models;

namespace SaleManagement.Open.Models
{
    public class SpotGoodsPatternViewModel
    {

        public SpotGoodsPatternViewModel(SpotGoodsPattern model)
        {
            Id = model.Id;
            Name = model.Name; 
            ImageUrl = SaleManagentConstants.Misc.SaleMangementWeb + "/Attachment/" + model.FileInfoId + "/preview";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public SpotGoodsType Type { get; set; }

        public string ImageUrl { get; set; }
    }
}