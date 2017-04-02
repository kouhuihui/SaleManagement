using Microsoft.Extensions.Logging;
using SaleManagement.Core;
using SaleManagement.Open.Models;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace SaleManagement.Open.Filter
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ApiException)
            {
                LoggerHelper.Logger.LogError(string.Format("调用规则错误\r\nUrl:{0}\r\nmessage{1}\r\n{2}",
                actionExecutedContext.Request.RequestUri, actionExecutedContext.Exception.Message,
                actionExecutedContext.Exception.StackTrace),
                actionExecutedContext.Exception);
                CreateApiExceptionResponse(actionExecutedContext);
            }
            else if (actionExecutedContext.Exception.GetType().IsSubclassOf(typeof(ArgumentException)))
            {
                LoggerHelper.Logger.LogError(string.Format("调用参数错误\r\nUrl:{0}\r\nmessage{1}\r\n{2}",
                actionExecutedContext.Request.RequestUri, actionExecutedContext.Exception.Message,
                actionExecutedContext.Exception.StackTrace),
                actionExecutedContext.Exception);
                CreateArgumentExceptionResponse(actionExecutedContext);
            }
            else
            {
                LoggerHelper.Logger.LogError(string.Format("服务内部错误\r\nUrl:{0}\r\nmessage{1}\r\n{2}",
                actionExecutedContext.Request.RequestUri, actionExecutedContext.Exception.Message,
                actionExecutedContext.Exception.StackTrace),
                actionExecutedContext.Exception);
                CreateServerErrorResponse(actionExecutedContext);
            }
        }

        private void CreateServerErrorResponse(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError,
                new ApiError { Code = 500, Message = "服务内部错误，请联系管理员。" });
        }

        void CreateArgumentExceptionResponse(HttpActionExecutedContext actionExecutedContext)
        {
            var apiException = actionExecutedContext.Exception as ArgumentException;
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest,
                new ApiError { Code = 400, Message = "参数错误" });
        }

        static void CreateApiExceptionResponse(HttpActionExecutedContext actionExecutedContext)
        {
            var apiException = actionExecutedContext.Exception as ApiException;
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(apiException.StatusCode, apiException.ToModel());
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Exception is ApiException)
            {
                return Task.Run(() =>
                {
                    CreateApiExceptionResponse(actionExecutedContext);
                });
            }
            else
                return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }
    }
}