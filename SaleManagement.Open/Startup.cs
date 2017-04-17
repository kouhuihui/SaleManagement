using AutoMapper;
using Dickson.Logging.EnterpriseLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.Owin;
using Owin;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Open.Models.SpotGood;

[assembly: OwinStartup(typeof(SaleManagement.Open.Startup))]

namespace SaleManagement.Open
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var factory = new LoggerFactory();
            factory.AddFile();
            LoggerHelper.Logger = factory.CreateLogger("SaleManagement");
            LoggerHelper.Logger.LogInformation("程序启动");
            ConfigureAutoMapper();
        }

        void ConfigureAutoMapper()
        {
            Mapper.CreateMap<SpotGoodsOrder, SpotGoodsOrderViewModel>();
            Mapper.CreateMap<SpotGoodsOrderViewModel, SpotGoodsOrder>();
        }
    }
}
