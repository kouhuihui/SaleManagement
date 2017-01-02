using AutoMapper;
using Dickson.Core.ComponentModel;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.RepairOrder;
using SaleManagement.Protal.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class RepairOrderController : PortalController
    {
        public ActionResult Create(string shipmentOrderId)
        {
            var repairOrderViewModel = new RepairOrderViewModel
            {
                ShipmentOrderId = shipmentOrderId
            };
            return View(repairOrderViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Create(RepairOrderViewModel request)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var repairOrder = Mapper.Map<RepairOrderViewModel, RepairOrder>(request);

            var manager = new RepairOrderManager(User);
            var result = await manager.CreateRepairOrder(repairOrder);
            if (result.Succeeded)
            {
                await UpdateShipmentOrderTotalAmount(request.ShipmentOrderId, repairOrder.TotalAmount);
            }
            return Json(result);
        }

        public ActionResult Edit(string Id)
        {
            return View();
        }

        public async Task<ActionResult> GetRepairOrders(string shipmentOrderId)
        {
            var manager = new RepairOrderManager(User);
            var result = await manager.GetRepairOrders(shipmentOrderId);

            var repairOrders = result.Select(r =>
            {
                return Mapper.Map<RepairOrder, RepairOrderViewModel>(r);
            });

            return Json(true, string.Empty, repairOrders);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var manager = new RepairOrderManager(User);
            var repairOrder =await manager.GetRepairOrder(id);
            var shipmentOrderId = repairOrder.ShipmentOrderId;
            var difference = -repairOrder.TotalAmount;
            var result = await manager.DeleteAsync(repairOrder);
            if (result.Succeeded)
            {
                await UpdateShipmentOrderTotalAmount(shipmentOrderId, difference);
            }
            return Json(result);
        }

        private async Task<InvokedResult> UpdateShipmentOrderTotalAmount(string shipmentOrderId, double difference)
        {
            var manager = new ShipmentManager(User);
            var result = await manager.UpdateTotalAmountAsync(shipmentOrderId, difference);
            return result;
        }
    }
}