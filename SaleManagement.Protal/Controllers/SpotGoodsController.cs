using AutoMapper;
using Dickson.Core.ComponentModel;
using Dickson.Web.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Protal.Models;
using SaleManagement.Protal.Models.SpotGoods;
using SaleManagement.Protal.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class SpotGoodsController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(SpotGoodsQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View(request);

            var manager = new SpotGoodsManager(User);
            var paging = await manager.GetSpotGoodsListAsync(request.Start, request.Take, null);

            var spotGoodList = paging.List.Select(u =>
            {
                var spotGoods = Mapper.Map<SpotGoods, SpotGoodsListItemViewModel>(u);
                return spotGoods;
            });
            return Json(true, string.Empty, new
            {
                paging.Total,
                List = spotGoodList
            });
        }

        [PagingParameterInspector]
        public async Task<ActionResult> SellList(SpotGoodsQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View(request);

            var manager = new SpotGoodsManager(User);
            var paging = await manager.GetSpotGoodsOrderListAsync(request.Start, request.Take, null);

            var spotGoodList = paging.List.Select(u =>
            {
                var spotGoods = Mapper.Map<SpotGoodsOrder, SpotGoodsOrderViewModel>(u);
                return spotGoods;
            });
            return Json(true, string.Empty, new
            {
                paging.Total,
                List = spotGoodList
            });
        }

        public ActionResult Create()
        {
            var spotGoodViewModel = new SpotGoodsViewModel();
            return View(spotGoodViewModel);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var manager = new SpotGoodsManager(User);
            var spotGood = await manager.GetSpotGoods(id);

            var spotGoodViewModel = Mapper.Map<SpotGoods, SpotGoodsViewModel>(spotGood);
            return View("Create", spotGoodViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(SpotGoodsEditViewModel request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var basicDataManager = new BasicDataManager();
            var matchStones = await basicDataManager.GetMatchStonesAsync();

            JArray jarry = JArray.Parse(Request.Form["SetStoneInfos"]);
            List<SpotGoodsSetStoneInfo> SpotGoodsSetStoneInfos = new List<SpotGoodsSetStoneInfo>();

            var spotGoods = Mapper.Map<SpotGoodsEditViewModel, SpotGoods>(request);

            var serialNumberManager = new SerialNumberManager(User);
            var manager = new SpotGoodsManager(User);
            if (string.IsNullOrEmpty(spotGoods.Id))
            {
                spotGoods.Id = SaleManagentConstants.Misc.SpotGoodsPrefix + await serialNumberManager.NextSNAsync(SaleManagentConstants.SerialNames.SpotGoods);
            }
            for (int i = 0; i < jarry.Count; ++i)  //遍历JArray  
            {
                var setStoneInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<SpotGoodsSetStoneInfo>(jarry[i].ToString());
                var matchStone = matchStones.FirstOrDefault(r => r.Id == setStoneInfo.MatchStoneId);
                if (matchStone == null)
                    break;

                setStoneInfo.Price = matchStone.Price;
                setStoneInfo.WorkingCost = (int)matchStone.WorkingCost * setStoneInfo.Number;
                setStoneInfo.CreatorId = User.Id;
                setStoneInfo.SpotGoodsId = spotGoods.Id;
                SpotGoodsSetStoneInfos.Add(setStoneInfo);
            }
            spotGoods.SetStoneInfos = SpotGoodsSetStoneInfos;
            var result = await manager.CreateSpotGoods(spotGoods);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Edit(SpotGoodsEditViewModel request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new SpotGoodsManager(User);
            var spotGoods = Mapper.Map<SpotGoodsEditViewModel, SpotGoods>(request);
            var result = await manager.CreateSpotGoods(spotGoods);
            return Json(result);
        }

        public async Task<ActionResult> Detail(string id)
        {
            Requires.NotNullOrEmpty("id", nameof(id));

            var manager = new SpotGoodsManager(User);
            var spotGoods = await manager.GetSpotGoods(id);
            var spotGoodViewModel = Mapper.Map<SpotGoods, SpotGoodsViewModel>(spotGoods);

            return View(spotGoodViewModel);
        }

        public async Task<ActionResult> AddSetStone()
        {
            var manager = new BasicDataManager();
            var matchStones = await manager.GetMatchStonesAsync();
            ViewBag.matchStones = matchStones;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddSetStone(SpotGoodsSetStoneInfoViewModel spotGoodsSetStoneInfoViewModel)
        {
            var manager = new SpotGoodsManager(User);
            var spotGoodsSetStoneInfo = Mapper.Map<SpotGoodsSetStoneInfoViewModel, SpotGoodsSetStoneInfo>(spotGoodsSetStoneInfoViewModel);
            var basicDataManager = new BasicDataManager();
            var matchStone = await basicDataManager.GetMatchStoneAsync(spotGoodsSetStoneInfoViewModel.MatchStoneId);
            if (matchStone == null)
                return Json(false, SaleManagentConstants.Errors.InvalidRequest);

            spotGoodsSetStoneInfo.MatchStoneName = matchStone.Name;
            spotGoodsSetStoneInfo.Price = matchStone.Price;
            spotGoodsSetStoneInfo.CreatorId = User.Id;
            var result = await manager.AddSpotGoodsSetStoneInfo(spotGoodsSetStoneInfo);
            return Json(true, data: result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteSetStone(string spotGoodsId, int id)
        {
            var manager = new SpotGoodsManager(User);
            var result = await manager.DeleteSpotGoodsSetStoneInfo(spotGoodsId, id);
            return Json(result);
        }
    }
}