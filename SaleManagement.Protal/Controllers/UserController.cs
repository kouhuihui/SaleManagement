using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Protal.Models;
using SaleManagement.Protal.Models.User;
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
        public async Task<ActionResult> List(UserQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new UserManager();
            var paging = await manager.GetUsersAsync(request.Start, request.Take, request.GetUseristQueryFilter());

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
        [ValidateAntiForgeryToken]
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
                Mobile = model.Mobile,
                Status = UserStatus.Normal,
                IdentityType = IdentityType.Employee,
                CompanyId = User.CompanyId,
                RoleId = model.RoleId
            };

            var result = await new UserManager().RegisterAsync(user, model.Password);

            return Json(result);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var manager = new UserManager();
            var user = await manager.FindByIdAsync(id);
            var roles = await new RoleManager(User).GetRolesAsync();
            ViewBag.SystemRoles = new SelectList(roles, "Id", "Name", user.RoleId);
            //    Select(r => new SelectListItem
            //{
            //    Text = r.Name,
            //    Value = r.Id.ToString(),
            //    Selected = r.Id == user.RoleId
            //}).ToList();
            return View(new SaleUserEditViewModel(user));
        }

        [HttpPost]
        public async Task<JsonResult> Edit(SaleUserEditViewModel model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new UserManager();
            var user = await manager.FindByIdAsync(model.Id);
            user.Mobile = model.Mobile;
            user.Name = model.Name;
            user.Email = model.Email;
            user.RoleId = model.RoleId;

            var result = await manager.UpdateAsync(user);
            return Json(result);
        }

        public ActionResult ResetPassword(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Error(SaleManagement.Core.SaleManagentConstants.Errors.InvalidRequest);

            var model = new ResetPasswordRequest();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var manager = new UserManager();
            var result = await manager.ResetPasswordAsync(model.UserId, model.Password);
            return Json(result);
        }

        public async Task<JsonResult> GetUsersByRole(string roleCode)
        {
            var manager = new UserManager();
            var users = await manager.GetUserByRoleAsync(roleCode);
            return Json(true, data: users.Select(u => new SaleUserViewModel(u)));
        }

        public async Task<JsonResult> UpdateUserStatus([NamedModelBinder(typeof(CommaSeparatedModelBinder), "userIds")] string[] userIds, UserStatus status)
        {
            var manager = new UserManager();
            var result = await manager.UpdateUserStatus(userIds, status);
            return Json(result);
        }
    }
}