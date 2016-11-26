using Microsoft.AspNet.Identity;
using SaleManagement.Managers;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class RoleController : PortalController
    {
        // GET: Role
        [UrlAuthorize]
        public async Task<ActionResult> List()
        {
            var manager = new RoleManager(User);
            var roles =await manager.GetRolesAsync();
            return View(roles);
        }
    }
}