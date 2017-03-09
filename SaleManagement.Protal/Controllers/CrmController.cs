using Dickson.Web.Mvc.ModelBinding;
using SaleManagement.Core.Models;
using SaleManagement.Managers;
using SaleManagement.Protal.Models.Customer;
using SaleManagement.Protal.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SaleManagement.Protal.Controllers
{
    public class CrmController : PortalController
    {
        // GET: Crm
        public ActionResult Index()
        {
            return View();
        }

        [UrlAuthorize]
        [PagingParameterInspector]
        public async Task<ActionResult> List(CustomerQueryRequest request)
        {
            if (!Request.IsAjaxRequest())
                return View();

            var manager = new UserManager();
            var paging = await manager.GetCustomersAsync(request.Start, request.Take, request.GetCustomerListQueryFilter(User));

            var customerIds = paging.List.Select(c => c.Id);

            var dicountRateManager = new DiscountRateManager();
            var dicountRates = await dicountRateManager.GetCustomerDiscountRatesAsync(customerIds);

            var customerInfoManager = new CustomerInfoManager(User);
            var customerInfos = await customerInfoManager.GetCustomerInfosRatesAsync(customerIds);

            var customers = paging.List.Select(u =>
            {
                var customerViewModel = new CustomerViewModel(u);
                var dicountRate = dicountRates.FirstOrDefault(r => r.CustomerId == u.Id);
                if (dicountRate != null)
                {
                    customerViewModel.PriceOfWorkDiscountRate = dicountRate.PriceOfWork;
                    customerViewModel.SideStoneDiscountRate = dicountRate.SideStone;
                    customerViewModel.StoneSetterDiscountRate = dicountRate.StoneSetter;
                    customerViewModel.Loss18KRate = dicountRate.Loss18K;
                    customerViewModel.LossPtRate = dicountRate.LossPt;
                }

                var customerInfo = customerInfos.FirstOrDefault(r => r.UserId == u.Id);
                if (customerInfo != null)
                {
                    customerViewModel.Address = customerInfo.Address;
                }

                return customerViewModel;
            });

            return Json(true, string.Empty, new
            {
                paging.Total,
                List = customers,
            });
        }

        public async Task<JsonResult> Customers()
        {
            var customers = await new UserManager().GetAllCustomersAsync();
            var items = customers.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString(),
                Selected = a.Id == User.Id
            }).OrderByDescending(o => o.Selected);
            return Json(true, string.Empty, items);
        }

        public async Task<ActionResult> Create()
        {
            var customerUserEditViewModel = new CustomerUserEditViewModel();
            var roleManager = new RoleManager(User);
            var role = await roleManager.GetRoleAsync(SaleManagement.Core.SaleManagentConstants.SystemRole.CommonUser);
            customerUserEditViewModel.RoleId = role.Id;
            return View(customerUserEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(CustomerUserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false, data: ErrorToDictionary());

            var user = new SaleUser
            {
                Email = model.Email,
                UserName = model.UserName,
                Name = model.Name,
                EmailConfirmed = true,
                Telephone = model.Telephone,
                Mobile = model.Mobile,
                Status = UserStatus.Normal,
                IdentityType = IdentityType.Customer,
                CompanyId = User.CompanyId,
                RoleId = model.RoleId
            };

            var result = await new UserManager().RegisterAsync(user, model.Password);
            if (result.Succeeded)
            {
                model.Id = user.Id;
                await SaveCustomerDiscountRate(model);
                await SaveCustomerInfo(model);
            }
            return Json(result);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var manager = new UserManager();
            var user = await manager.FindByIdAsync(id);

            var customerUserEditViewModel = new CustomerUserEditViewModel(user);
            var discountRateManager = new DiscountRateManager();
            var discountRate = await discountRateManager.GetCustomerDiscountRateAsync(user.Id);
            if (discountRate != null)
            {
                customerUserEditViewModel.PriceOfWorkDiscountRate = discountRate.PriceOfWork;
                customerUserEditViewModel.SideStoneDiscountRate = discountRate.SideStone;
                customerUserEditViewModel.StoneSetterDiscountRate = discountRate.StoneSetter;
                customerUserEditViewModel.Loss18KRate = discountRate.Loss18K;
                customerUserEditViewModel.LossPtRate = discountRate.LossPt;
            }

            var customerInfoManager = new CustomerInfoManager(User);
            var customerInfo = await customerInfoManager.GetCustomerInfoAsync(user.Id);
            if (customerInfo != null)
            {
                customerUserEditViewModel.Address = customerInfo.Address;
            }

            return View(customerUserEditViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Edit(CustomerUserEditViewModel model)
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

            var result = await manager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await SaveCustomerDiscountRate(model);
                await SaveCustomerInfo(model);
            }
            return Json(result);
        }

        private async Task SaveCustomerDiscountRate(CustomerUserEditViewModel model)
        {
            var manager = new DiscountRateManager();
            var discountRate = await manager.GetCustomerDiscountRateAsync(model.Id);

            discountRate.CreatorId = User.Id;
            discountRate.Created = DateTime.Now;
            discountRate.CustomerId = model.Id;
            discountRate.PriceOfWork = model.PriceOfWorkDiscountRate;
            discountRate.SideStone = model.SideStoneDiscountRate;
            discountRate.StoneSetter = model.StoneSetterDiscountRate;
            discountRate.LossPt = model.LossPtRate;
            discountRate.Loss18K = model.Loss18KRate;

            await manager.SaveDiscountRateAsync(discountRate);
        }

        private async Task SaveCustomerInfo(CustomerUserEditViewModel model)
        {
            var manager = new CustomerInfoManager(User);
            var customerInfo = await manager.GetCustomerInfoAsync(model.Id);
            if (customerInfo == null)
            {
                customerInfo = new CustomerInfo();
            }

            customerInfo.UserId = model.Id;
            customerInfo.Address = model.Address;

            await manager.SaveCustomerInfoAsync(customerInfo);
        }
    }
}