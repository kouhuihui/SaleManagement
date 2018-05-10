using System.Configuration;

namespace SaleManagement.Core.ViewModel
{
    public class MesBase
    {
        private static readonly string _AppId = ConfigurationManager.AppSettings["AppID"];
        private static readonly string _AppSecret = ConfigurationManager.AppSettings["AppSecret"];

        public MesBase()
        {
            AppId = _AppId;
            AppSecret = _AppSecret;
        }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string OpenId { get; set; }

    }
}
