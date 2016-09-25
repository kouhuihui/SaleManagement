using Dickson.Web.Extensions;
using Microsoft.Owin;
using Newtonsoft.Json;
using SaleManagement.Core;
using SaleManagement.Protal.Models;
using System;
using System.Configuration;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using SaleManagement.Managers;
using SaleManagement.Store;
using System.Threading.Tasks;
using Dickson.Library.Threading;

namespace SaleManagement.Protal.Web
{
    public class WeChatAttribute : FilterAttribute, IActionFilter
    {
        private static string _AppId = ConfigurationManager.AppSettings["AppID"];
        private static string _AppSecret = ConfigurationManager.AppSettings["AppSecret"];
        private static string _OauthRedirecturi = ConfigurationManager.AppSettings["OauthRedirecturi"];

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var owinContext = filterContext.HttpContext.GetOwinContext();
            var isWechat = owinContext.GetBrowser().IsWeChat;
            if (!isWechat)
                return;

            var cookie = owinContext.Request.Cookies[SaleManagentConstants.ConfigKeys.wxAccountCookie];
            if (cookie != null)
                return;

            try
            {
                //微信授权获得code
                var code = owinContext.Request.Query.Get("code");
                if (string.IsNullOrEmpty(code))
                {
                    LoggerHelper.Logger.LogInformation("跳转到微信授权");
                    filterContext.HttpContext.Response.Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=123#wechat_redirect",
                        _AppId, System.Web.HttpUtility.UrlEncode(_OauthRedirecturi)));
                    return;
                }
                else //通过code获取openId
                {
                    LoggerHelper.Logger.LogInformation($"微信授权成功获取code:{code}");
                    var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}", _AppId, _AppSecret, code, "authorization_code");
                    OAuthAccessTokenResult model = new OAuthAccessTokenResult();
                    using (var client = new HttpClient())
                    {
                        var response = client.GetAsync(url).Result;
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                            return;

                        var result = response.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<OAuthAccessTokenResult>(result);
                        response.Dispose();
                    }

                    if (string.IsNullOrEmpty(model.openid))
                        return;
                    LoggerHelper.Logger.LogInformation($"微信获取openId成功:{model.openid}");
                    var date = DateTime.Now.Date;
                    var options = new CookieOptions { Expires = date.AddDays(SaleManagentConstants.UI.DefaultExpiringDays) };
                    owinContext.Response.Cookies.Append(SaleManagentConstants.ConfigKeys.wxAccountCookie,
                        model.openid, options);

                    var accountBindingManager = new AccountBindingManager();
                    var accountBing = AsyncHelper.RunSync(async () => await accountBindingManager.GetAccountBindingAsync(model.openid));
                    if (accountBing == null)
                        return;

                    var manager = new SignInManager(new SaleUserStore());
                    var signInResult =  AsyncHelper.RunSync(async () => await manager.UserNameSignInAsync(owinContext.Authentication, accountBing.UserName, false));
                    if (signInResult == SignInResult.Success)
                        filterContext.Result = new RedirectResult("/Home/Index");
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Logger.LogError(e.InnerException.ToString());
                return;
            }
        }
    }
}