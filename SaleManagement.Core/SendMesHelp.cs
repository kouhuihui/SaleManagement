using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SaleManagement.Core.ViewModel;
using System;
using System.Configuration;

namespace SaleManagement.Core
{
    public class SendMesHelp
    {

        private static string _WeixinUrl = ConfigurationManager.AppSettings["WeixinUrl"];

        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="mes"></param>
        public static void SendNews(NewsMes mes)
        {
            var pars = JsonConvert.SerializeObject(mes).ToString();
            try
            {
                new RestHelp().QueryPostRestService(_WeixinUrl + "Custom/SendNews", pars);
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.LogInformation("发送消息失败" + pars + "原因：" + ex.Message);
            }

        }
    }
}
