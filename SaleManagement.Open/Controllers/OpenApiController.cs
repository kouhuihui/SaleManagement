using SaleManagement.Open.Filter;
using SaleManagement.Open.Models;
using SaleManagement.Open.Results;
using System.Web.Http;

namespace SaleManagement.Open.Controllers
{
    [ApiExceptionFilter]
    public class OpenApiController : ApiController
    {
        protected NotFoundNegotiatedContentResult<T> NotFound<T>(T content)
        {
            return new NotFoundNegotiatedContentResult<T>(content, this);
        }

        protected NotFoundNegotiatedContentResult<ApiError> NotFound(int errorCode, string message)
        {
            var error = new ApiError { Code = errorCode, Message = message };
            return new NotFoundNegotiatedContentResult<ApiError>(error, this);
        }

        protected BadRequestNegotiatedContentResult<T> BadRequest<T>(T content)
        {
            return new BadRequestNegotiatedContentResult<T>(content, this);
        }
    }
}
