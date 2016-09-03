using SaleManagement.Managers;
using SaleManagement.Protal.Models;
using SaleManagement.Protal.Models.Menu;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class SystemMenuController : PortalController
    {
        // GET: SystemMenu
        public async Task<ActionResult> Authorized(int roleId)
        {
            var manager = new SystemMenuManager();
            var menus = await manager.GetSystemMenusAsync();

            var roleMenuManager = new RoleMenuManager();
            var roleMenus = await roleMenuManager.GetRoleMenusAsync(roleId);
            var viewModel = new RoleMenuAuthorizedViewModel
            {
                RoleMenus = roleMenus,
                SystemMenus = menus,
                RoleId = roleId
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Authorized(int[] menuId, int roleId)
        {
            var roleMenuManager = new RoleMenuManager();
            var result = await roleMenuManager.SaveRoleMenusAsync(menuId, roleId);
            return Json(result);
        }
    }
}