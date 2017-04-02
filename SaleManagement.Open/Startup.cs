using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Extensions.Logging;
using Dickson.Logging.EnterpriseLibrary;
using SaleManagement.Core;

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
        }
    }
}
