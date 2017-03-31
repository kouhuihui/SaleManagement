using SaleManagement.Managers;
using SaleManagement.Open.Models.SpotGood;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SaleManagement.Open.Controllers
{
    [RoutePrefix("SpotGood")]
    public class SpotGoodController : ApiController
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

    }
}
