using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models;
using SaleManagement.Protal.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class UserController : PortalController
    {
        // GET: User
        [PagingParameterInspector]
        public async Task<ActionResult> List(PagingRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new UserManager();
            var paging = await manager.GetUsersAsync(request.Start, request.Take);

            var users = paging.List.Select(u => new SaleUserViewModel(u));

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = users,
            });
        }

        public async Task<ActionResult> Create()
        {
            var model = new SaleUserEditViewModel();
            var roles = await new RoleManager(User).GetRolesAsync();
            ViewBag.Roles = roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(SaleUserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var user = new SaleUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = model.Email,
                UserName = model.UserName,
                Name = model.Name,
                EmailConfirmed = true,
                Telephone = model.Telephone,
                Status = UserStatus.Normal,
                IdentityType = IdentityType.Employee,
                CompanyId = User.CompanyId,
                RoleId = model.RoleId
            };

            var result = await new UserManager().RegisterAsync(user, model.Password);

            return Json(result);
        }

        public async Task<JsonResult> GetUsersByRole(string roleCode)
        {
            var manager = new UserManager();
            var users = await manager.GetUserByRoleAsync(roleCode);
            return Json(true, data: users.Select(u => new SaleUserViewModel(u)));
        }
    }
}