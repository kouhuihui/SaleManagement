using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web;
using System.Web.Mvc;

namespace Dickson.Web.Mvc
{
    public class JsonNetResult : JsonResult
    {
        static JsonSerializerSettings _DefaultSetting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("默认情况下不允许用GET方法发出Ajax请求，请设置JsonRequestBehavior属性为AllowGet。");

            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                response.Write(JsonConvert.SerializeObject(Data, Settings ?? _DefaultSetting));
            }
        }

        public JsonSerializerSettings Settings
        {
            get;
            set;
        }

        public static JsonSerializerSettings DefaultSettings
        {
            get
            {
                return _DefaultSetting;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _DefaultSetting = value;
            }
        }
    }
}
