using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models;

namespace webapi.Controllers
{
    public class ClassesController : ApiController
    {
        onlineQuranTutorEntities4 _context = new onlineQuranTutorEntities4();
        [HttpPost]
        public HttpResponseMessage updateClass(UpdateClass classDTO)
        {
            if (string.IsNullOrEmpty(classDTO.status) || classDTO.classID <= 0 || classDTO.corrections < 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Invalid Data" });
            }
            var classData = _context.Classes.Where(c => c.classID == classDTO.classID).FirstOrDefault();
            classData.status = classDTO.status;
            classData.corrections = classDTO.corrections;
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "updated successfully" });
        }
    }
}
