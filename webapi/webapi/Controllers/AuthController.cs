using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Net;
namespace webapi.Controllers
{
    public class AuthController : ApiController
    {
        onlineQuranTutorEntities4 _context = new onlineQuranTutorEntities4();
        [HttpPost]
        public HttpResponseMessage checkEmail(string email)
        {

            var user = _context.Users.Where(u => u.email == email).FirstOrDefault();
            if (user != null)

                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Email checked successfully"
                });

            return Request.CreateResponse(new
            {
                statusCode = HttpStatusCode.OK,
                message = "Email checked successfully"
            });
        }


        [HttpPut]
        public HttpResponseMessage updatePassword(int userID, string newPassword)
        {
            var user = _context.Users.Where(u => u.userID == userID).FirstOrDefault();
            if (user != null)
            {
                user.password = newPassword;
                _context.SaveChanges();
                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Password updated successfully"
                });
            }
            return Request.CreateResponse(new
            {
                statusCode = HttpStatusCode.NotFound,
                message = "User not found"
            });
        }
    }

}
