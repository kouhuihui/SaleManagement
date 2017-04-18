using AutoMapper;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Open.Models;
using SaleManagement.Open.Models.SpotGood;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SaleManagement.Open.Controllers
{
    [RoutePrefix("SpotGood")]
    public class SpotGoodController : OpenApiController
    {
        [Route("~/SpotGoods")]
        [HttpPost]
        public async Task<IHttpActionResult> SpotGoods(SpotGoodsQueryRequest request)
        {
            var manager = new SpotGoodsManager();
            var spotGoods = await manager.GetSpotGoodsAsync(request.GetSpotGoodsListQueryFilter());

            if (spotGoods == null)
                return NotFound("现货不存在");

            var spotGoodsViewModel = new SpotGoodListItemViewModel(spotGoods);
            var dailyGoldPriceManager = new DailyGoldPriceManager();
            var dailyGoldPrice = await dailyGoldPriceManager.GetNewDailyGoldPriceAsync(spotGoodsViewModel.ColorFormId);
            spotGoodsViewModel.DailyGoldPrice = dailyGoldPrice.Price;
            if (spotGoodsViewModel.Price == 0)
            {
                spotGoodsViewModel.Price = decimal.Round(((decimal)(dailyGoldPrice.Price * spotGoodsViewModel.GoldWeight + spotGoods.SetStoneInfos.Sum(r => r.Price * r.Weight + r.Number * r.WorkingCost))), 2);
            }
            return Ok(spotGoodsViewModel);
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

        [Route("ColorForms")]
        public async Task<IHttpActionResult> ColorForms(string patternId)
        {
            var manager = new SpotGoodsManager();
            var colorFormList = await manager.GetSpotGoodsColorFromListAsync(patternId);
            return Ok(colorFormList);
        }

        [Route("MainStones")]
        public async Task<IHttpActionResult> MainStones(string patternId, int colorFromId)
        {
            var manager = new SpotGoodsManager();
            var mainStoneList = await manager.GetSpotGoodsMainStoneListAsync(patternId, colorFromId);
            return Ok(mainStoneList);
        }

        [Route("HandSizes")]
        public async Task<IHttpActionResult> HandSizes(string patternId, int colorFromId, string mainStone)
        {
            var manager = new SpotGoodsManager();
            var handSizeList = await manager.GetSpotGoodsHandSizeListAsync(patternId, colorFromId, mainStone);
            return Ok(handSizeList);
        }

        [Route("UpdateLockStatus")]
        public async Task<IHttpActionResult> UpdateLockStatus(string orderId, bool isLock)
        {
            var manager = new SpotGoodsManager();
            var result = await manager.UpdateSpotGoodLock(orderId, isLock);
            return Ok(result);
        }

        [Route("~/SpotGoodsOrder/Create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(SpotGoodsOrderViewModel request)
        {
            var manager = new SpotGoodsManager();
            var spotGoodsOrder = Mapper.Map<SpotGoodsOrderViewModel, SpotGoodsOrder>(request);
            spotGoodsOrder.Created = DateTime.Now;
            var result = await manager.CreateSpotGoodsOrder(spotGoodsOrder);

            return Ok(result);
        }

        [Route("~/SpotGoodsOrder/CustomerInfo/Update")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateOrderCustomerInfo(CustomerAddressViewModel request)
        {
            var manager = new SpotGoodsManager();
            var result = await manager.UpdateOrderCustomerInfo(request.SpotGoodsId, request.Address, request.CustomerPhone, request.CustomerName, request.IsSF);

            return Ok(result);
        }

        public async Task<IHttpActionResult> GetSpotGoodsOrders(string openId, LogisticType logisticType)
        {
            var manager = new SpotGoodsManager();

            Func<IQueryable<SpotGoodsOrder>, IQueryable<SpotGoodsOrder>> filter = query =>
            {
                query = query.Where(r => r.OpenId == openId);
                if (logisticType == LogisticType.Self)
                {
                    query = query.Where(r => r.SpotGoods.Status == SpotGoodsStatus.PickBySelf || r.SpotGoods.Status == SpotGoodsStatus.HasTaken);
                }
                if (logisticType == LogisticType.SF)
                {
                    query = query.Where(r => r.SpotGoods.Status == SpotGoodsStatus.SF || r.SpotGoods.Status == SpotGoodsStatus.HasSendGoods);
                }
                return query;
            };
            var result = await manager.GetSpotGoodsOrderListAsync(filter);
            var orders = result.Select(r => new SpotGoodListItemViewModel(r)).ToList();

            return Ok(orders);
        }

    }
}
