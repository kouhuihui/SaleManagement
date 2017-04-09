using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace SaleManagement.Open.Results
{
    public class BadRequestNegotiatedContentResult<T> : OkNegotiatedContentResult<T>
    {
        public BadRequestNegotiatedContentResult(T content, ApiController controller)
            : base(content, controller)
        {

        }

        public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(cancellationToken).ContinueWith(t =>
            {
                t.Result.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return t.Result;
            });
        }
    }
}