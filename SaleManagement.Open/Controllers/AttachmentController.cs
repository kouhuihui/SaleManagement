using Dickson.Core.ComponentModel;
using SaleManagement.Managers;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace SaleManagement.Open.Controllers
{
    public class AttachmentController : OpenApiController
    {
        [HttpGet]
        [Route("~/Attachment/{fileId}/Preview")]
        public async Task<HttpResponseMessage> Preview(string fileId)
        {
            Requires.NotNullOrEmpty(fileId, "fileId");

            var fileManager = new FileManager();
            var file = await fileManager.FindByIdAsync(fileId);
            if (file == null && file.Data != null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(file.Data);  //data为二进制图片数据
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            return response;
        }
    }
}
