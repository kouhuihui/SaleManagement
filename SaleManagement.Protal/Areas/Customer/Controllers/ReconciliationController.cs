using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core.ViewModel;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Reconciliation;
using SaleManagement.Protal.Web;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Areas.Customer.Controllers
{
    public class ReconciliationController : PortalController
    {
        [PagingParameterInspector]
        public async Task<ActionResult> List()
        {
            var manager = new ReconciliationManager(User);

            var reconciliations = await manager.GetCustomerReconciliationsAsync();
            return View(reconciliations);
        }
    }
}