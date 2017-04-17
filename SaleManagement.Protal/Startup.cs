using AutoMapper;
using Dickson.Core.Common.Extensions;
using Dickson.Logging.EnterpriseLibrary;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Protal.Models;
using SaleManagement.Protal.Models.Order;
using SaleManagement.Protal.Models.RepairOrder;
using SaleManagement.Protal.Models.Shipment;
using SaleManagement.Protal.Models.SpotGoods;
using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartupAttribute(typeof(SaleManagement.Protal.Startup))]
namespace SaleManagement.Protal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleTable.Bundles.RegisterConfigurationBundles();
            ////BundleTable.EnableOptimizations = true;
            ConfigureAutoMapper();
            AntiForgeryConfig.UniqueClaimTypeIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var factory = new LoggerFactory();
            factory.AddFile();
            LoggerHelper.Logger = factory.CreateLogger("SaleManagement");
            LoggerHelper.Logger.LogInformation("程序启动");
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookiePath = "/",
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout"),
                ExpireTimeSpan = TimeSpan.FromDays(7)
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }

        static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "SaleManagement.Protal.Controllers" }
            );
        }

        void ConfigureAutoMapper()
        {
            Mapper.CreateMap<OrderEditViewModel, Order>();
            Mapper.CreateMap<OrderSetStoneInfoViewModel, OrderSetStoneInfo>();
            Mapper.CreateMap<ShipmentOrderViewModel, ShipmentOrder>();
            Mapper.CreateMap<ShipmentOrderInfoViewModel, ShipmentOrderInfo>();
            Mapper.CreateMap<ShipmentOrder, ShipmentOrderViewModel>()
                .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate.ToString(SaleManagentConstants.UI.DateStringFormat)))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created.ToString(SaleManagentConstants.UI.DateStringFormat)));
            Mapper.CreateMap<ShipmentOrderInfo, ShipmentOrderInfoViewModel>();
            Mapper.CreateMap<RepairOrder, RepairOrderViewModel>();
            Mapper.CreateMap<RepairOrderViewModel, RepairOrder>();
            Mapper.CreateMap<SpotGoodsEditViewModel, SpotGoods>();
            Mapper.CreateMap<SpotGoods, SpotGoodsViewModel>();
            Mapper.CreateMap<SpotGoodsSetStoneInfo, SpotGoodsSetStoneInfoViewModel>();
            Mapper.CreateMap<SpotGoodsSetStoneInfoViewModel, SpotGoodsSetStoneInfo>();
            Mapper.CreateMap<SpotGoods, SpotGoodsListItemViewModel>()
                 .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created.ToString(SaleManagentConstants.UI.DateStringFormat)))
                 .ForMember(dest => dest.ColorFormName, opt => opt.MapFrom(src => src.ColorForm.Name))
                 .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.GetDisplayName()))
                 .ForMember(dest => dest.SpotGoodsPatternName, opt => opt.MapFrom(src => src.SpotGoodsPattern.Name))
                 .ForMember(dest => dest.SpotGoodTypeName, opt => opt.MapFrom(src => src.SpotGoodsPattern.Type.GetDisplayName()));
            Mapper.CreateMap<SpotGoodsOrder, SpotGoodsOrderViewModel>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created.ToString(SaleManagentConstants.UI.DateStringFormat)))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.SpotGoods.Status.GetDisplayName()));
        }
    }
}
