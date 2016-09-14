using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Protal.Web;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    [RoutePrefix("BasicData")]
    public class BasicDataController : PortalController
    {
        // GET: BasicInfo
        public async Task<ActionResult> ProductCategories()
        {
            var manager = new BasicDataManager(User);
            var categories = await manager.GetProductCategoriesAsync();
            return View(categories);
        }

        public async Task<ActionResult> EditProductCategory(int? id)
        {
            var productCategory = new ProductCategory();
            if (!id.HasValue)
                return View(productCategory);

            var manager = new BasicDataManager(User);
            productCategory = await manager.GetProductCategoryAsync(id.Value);

            return View(productCategory);
        }

        [HttpPost]
        public async Task<JsonResult> EditProductCategory(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new BasicDataManager(User);
            var result = await manager.SaveProductCategoryAsync(productCategory);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveProductCategory(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            var manager = new BasicDataManager(User);
            var result = await manager.DeleteProductCategoryAsync(id);

            return Json(result);
        }

        public async Task<ActionResult> MatchStones()
        {
            var manager = new BasicDataManager(User);
            var matchStones = await manager.GetMatchStonesAsync();
            return View(matchStones);
        }

        public async Task<ActionResult> EditMatchStone(int? id)
        {
            var matchStone = new MatchStone();
            if (!id.HasValue)
                return View(matchStone);

            var manager = new BasicDataManager(User);
            matchStone = await manager.GetMatchStoneAsync(id.Value);

            return View(matchStone);
        }

        [HttpPost]
        public async Task<JsonResult> EditMatchStone(MatchStone matchStone)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new BasicDataManager(User);
            var result = await manager.SaveMatchStoneAsync(matchStone);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveMatchStone(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            var manager = new BasicDataManager(User);
            var result = await manager.DeleteMatchStoneAsync(id);

            return Json(result);
        }

        public async Task<ActionResult> GemCategories()
        {
            var manager = new BasicDataManager(User);
            var categories = await manager.GetGemCategoriesAsync();
            return View(categories);
        }

        public async Task<ActionResult> EditGemCategory(int? id)
        {
            var gemCategory = new GemCategory();
            if (!id.HasValue)
                return View(gemCategory);

            var manager = new BasicDataManager(User);
            gemCategory = await manager.GetGemCategoryAsync(id.Value);
            return View(gemCategory);
        }

        [HttpPost]
        public async Task<JsonResult> EditGemCategory(GemCategory gemCategory)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new BasicDataManager(User);
            var result = await manager.SaveGemCategoryAsync(gemCategory);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveGemCategory(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            var manager = new BasicDataManager(User);
            var result = await manager.DeleteGemCategoryAsync(id);

            return Json(result);
        }

        public async Task<ActionResult> ColorForms()
        {
            var manager = new BasicDataManager(User);
            var colorForms = await manager.GetColorFormsAsync();
            return View(colorForms);
        }

        public async Task<ActionResult> EditColorForm(int? id)
        {
            var colorForm = new ColorForm();
            if (!id.HasValue)
                return View(colorForm);

            var manager = new BasicDataManager(User);
            colorForm = await manager.GetColorFormAsync(id.Value);
            return View(colorForm);
        }

        [HttpPost]
        public async Task<JsonResult> EditColorForm(ColorForm colorForm)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new BasicDataManager(User);
            var result = await manager.SaveColorFormAsync(colorForm);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveColorForm(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();

            var manager = new BasicDataManager(User);
            var result = await manager.DeleteColorFormAsync(id);

            return Json(result);
        }

        public async Task<ActionResult> ShippingScheduleSetting()
        {
            var manager = new BasicDataManager(User);
            var shippingScheduleSetting = await manager.GetShippingScheduleSettingAsync();
            if (shippingScheduleSetting == null)
            {
                shippingScheduleSetting = new ShippingScheduleSetting();
            }
            return View(shippingScheduleSetting);
        }

        [HttpPost]
        public async Task<JsonResult> ShippingScheduleSetting(int days)
        {
            var manager = new BasicDataManager(User);
            var shippingScheduleSetting =await manager.GetShippingScheduleSettingAsync();
            if (shippingScheduleSetting == null)
            {
                shippingScheduleSetting = new ShippingScheduleSetting();
            }
            shippingScheduleSetting.Days = days;
            var result = await manager.SaveShippingScheduleSettingAsync(shippingScheduleSetting);
            return Json(result);
        }
    }
}