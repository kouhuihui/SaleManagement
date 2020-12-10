using SaleManagement.Protal.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Areas.Customer.Controllers
{
    public class HotSellingController : PortalController
    {
        // GET: Customer/HotSelling
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Detail(string id)
        {
            return View();
        }
    }
}