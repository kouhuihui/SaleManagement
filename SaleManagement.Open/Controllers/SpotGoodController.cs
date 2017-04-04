using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Open.Models;
using SaleManagement.Open.Models.SpotGood;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SaleManagement.Open.Controllers
{
    [RoutePrefix("SpotGood")]
    public class SpotGoodController : OpenApiController
    {
        [Route("~/SpotGoods")]
        public async Task<IHttpActionResult> SpotGoods(SpotGoodsQueryRequest request)
        {
            var manager = new SpotGoodsManager();
            var spotGoodsList = await manager.GetSpotGoodsListAsync(request.Start, request.Take, null);

            var spotGoodsViewModels = spotGoodsList.List.Select(u =>
            {
                var spotGoods = new SpotGoodListItemViewModel(u);
                return spotGoods;
            });
            return Ok(spotGoodsViewModels);
        }


        [Route("Patterns/{type}")]
        public async Task<IHttpActionResult> Patterns(SpotGoodsType type)
        {
            var manager = new SpotGoodsManager();
            var spotGoodsPatterns = await manager.GetSpotGoodsPatternListAsync(type);
            var spotGoodsPatternViewModels = spotGoodsPatterns.Select(u =>
            {
                return new SpotGoodsPatternViewModel(u);
            });

            return Ok(spotGoodsPatternViewModels);
        }

        [Route("MainStones")]
        public async Task<IHttpActionResult> MainStones(string patterId)
        {
            var manager = new SpotGoodsManager();
            var mainStoneList = await manager.GetSpotGoodsMainStoneListAsync(patterId);
            return Ok(mainStoneList);
        }

        [Route("HandSizes")]
        public async Task<IHttpActionResult> MainStones(string patterId, string mainStones)
        {
            var manager = new SpotGoodsManager();
            var handSizeList = await manager.GetSpotGoodsHandSizeListAsync(patterId);
            return Ok(handSizeList);
        }
    }
}
