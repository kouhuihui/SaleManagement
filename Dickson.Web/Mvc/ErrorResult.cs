using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dickson.Web.Mvc
{
    public class ErrorResult : ActionResult
    {
        string m_Message;
        string m_ViewName;

        internal string Message { get { return m_Message; } }

        internal string ViewName { get { return m_ViewName; } }

        public ErrorResult(string message, string viewName)
        {
            m_Message = message;
            m_ViewName = viewName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var request = context.HttpContext.Request;

            if (request.IsAjaxRequest())
            {
                ExecuteAjaxResult(context);
            }
            else
            {
                ExecuteCommonResult(context);
            }
        }

        protected virtual void ExecuteCommonResult(ControllerContext context)
        {
            context.Controller.ViewData.Model = m_Message;
            var result = new ViewResult
            {
                ViewName = m_ViewName,
                MasterName = null,
                ViewData = context.Controller.ViewData,
                TempData = context.Controller.TempData,
                ViewEngineCollection = ((Controller)context.Controller).ViewEngineCollection
            };
            result.ExecuteResult(context);
        }

        protected virtual void ExecuteAjaxResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            httpContext.Response.Clear();
            httpContext.Response.TrySkipIisCustomErrors = true;
            httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            var jsonData = AjaxResponse.Fail(m_Message);
            var result = new JsonResult { Data = jsonData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            result.ExecuteResult(context);
        }
    }
}