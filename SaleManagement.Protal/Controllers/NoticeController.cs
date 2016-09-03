using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Web;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class NoticeController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List(PagingRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new NoticeManager(User);
            var paging = await manager.GetNoticesAsync(request.Start, request.Take);

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = paging.List,
            });
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var manager = new NoticeManager(User);
            var notice = new Notice();
            if (id > 0)
            {
                notice = await manager.GetNoticeAsync(id);
            }

            return View(notice);
        }

        [HttpPost]
        public async Task<JsonResult> Edit(Notice notice)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new NoticeManager(User);
            var result = await manager.SaveNotice(notice);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var manager = new NoticeManager(User);
            var result = await manager.DeleteNoticeAsync(id);

            return Json(result);
        }
    }
}