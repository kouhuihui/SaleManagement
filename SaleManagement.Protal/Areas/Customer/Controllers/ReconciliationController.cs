using Dickson.Core.Common.Extensions;
using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core;
using SaleManagement.Managers;
using SaleManagement.Protal.Web;
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

        public async Task<FileStreamResult> Export()
        {
            var manager = new ReconciliationManager(User);
            var reconciliations = await manager.GetCustomerReconciliationsAsync();
            var titles = new string[] { "序号", "日期", "付/欠款", "金额(元)", "备注" };
            var result = Dickson.Web.Helper.ExcelHelp.Export(titles, "对账记录", ws =>
            {
                var row = 2;
                int index = 1;
                foreach (var reconciliation in reconciliations)
                {

                    ws.Cells[row, 1].Value = index;
                    ws.Cells[row, 2].Value = reconciliation.Created.ToString(SaleManagentConstants.UI.DateStringFormat);
                    ws.Cells[row, 3].Value = reconciliation.Type.GetDisplayName();
                    ws.Cells[row, 4].Value = reconciliation.Amount;
                    ws.Cells[row, 5].Value = reconciliation.Remark;
                    row++;
                    index++;
                };
            });
            return result;
        }
    }
}