using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class SpotGoodTypeController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(PagingRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View(request);

            var manager = new SpotGoodTypeManager(User);
            var paging = await manager.GetSpotGoodTypeListAsync(request.Start, request.Take, null);

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = paging.List.Select(r => new SpotGoodType
                {
                    Id = r.Id,
                    Name = r.Name
                })
            });
        }

        [HttpGet]
        public async Task<ActionResult> All()
        {
            var manager = new SpotGoodTypeManager(User);
            var spotGoodTypes = await manager.GetSpotGoodTypeListAsync();

            return Json(true, string.Empty, spotGoodTypes);
        }


        public ActionResult Add()
        {
            var model = new SpotGoodType();
            return View(model);
        }

        public async Task<ActionResult> Edit(string Id)
        {
            var manager = new SpotGoodTypeManager(User);
            var model = await manager.GetSpotGoodType(Id);
            return View("Add", model);
        }

        [HttpPost]
        public async Task<ActionResult> Save(SpotGoodType spotGoodType)
        {
            if (string.IsNullOrEmpty(spotGoodType.Id))
            {
                spotGoodType.Id = Guid.NewGuid().ToString();
            }
            var manager = new SpotGoodTypeManager(User);
            var result = await manager.SaveSpotGoodType(spotGoodType);
            return Json(result);
        }
    }
}