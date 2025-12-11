using System.Net.Http;
using System.Web.Http;
using System.Net;
using webapi.Models;
namespace webapi.Controllers
{
    public class GuardianController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage addChild(ChildDTO childdata)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                statusCode = System.Net.HttpStatusCode.OK,
                message = "Guardian login successful"
            });
        }
    }
}
