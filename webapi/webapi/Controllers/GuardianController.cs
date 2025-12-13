using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using webapi.Models;
namespace webapi.Controllers
{
    public class GuardianController : ApiController
    {
        onlineQuranTutorEntities4 _context = new onlineQuranTutorEntities4();
        [HttpPost]
        public HttpResponseMessage addChild()
        {







            var request = HttpContext.Current.Request;

            var json = request.Form["student"];


            if (string.IsNullOrEmpty(json))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Student JSON missing");

            ChildDTO childdata = JsonConvert.DeserializeObject<ChildDTO>(json);

            if (childdata.guardianID <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Guardian ID Error"
                });
            }



            if (request.Files.Count > 0)
            {
                var postedFile = request.Files[0];

                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    var extension = Path.GetExtension(postedFile.FileName);
                    string fileName = childdata.email + extension;

                    string serverPath = HttpContext.Current.Server.MapPath("~/images/");
                    string fullPath = Path.Combine(serverPath, fileName);

                    if (!Directory.Exists(serverPath))
                        Directory.CreateDirectory(serverPath);

                    postedFile.SaveAs(fullPath);

                    childdata.profilePicture = fileName;
                }
            }









            if (childdata == null ||
                   string.IsNullOrWhiteSpace(childdata.name) ||
                   string.IsNullOrWhiteSpace(childdata.email) ||
                   string.IsNullOrWhiteSpace(childdata.password) ||
                   string.IsNullOrWhiteSpace(childdata.gender) ||
                   childdata.dateOfBirth.ToString() == null ||
                   string.IsNullOrWhiteSpace(childdata.userType) ||
                   string.IsNullOrWhiteSpace(childdata.city) ||
                   string.IsNullOrWhiteSpace(childdata.timezone) ||
                   string.IsNullOrWhiteSpace(childdata.country))
            {
                return Request.CreateResponse(
                    HttpStatusCode.NotAcceptable, new
                    {
                        message = "All fields are required"
                    });
            }
            var child = _context.Users.Where(user => user.email == childdata.email).FirstOrDefault();
            if (child != null)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadGateway, new
                    {
                        message = $"Child already exists with this {childdata.email}"
                    });
            }

            _context.Users.Add(new User()
            {
                name = childdata.name,
                email = childdata.email,
                password = childdata.password,
                gender = childdata.gender,
                country = childdata.country,
                timezone = childdata.timezone,
                city = childdata.city,
                dateOfBirth = childdata.dateOfBirth,
                userType = childdata.userType,
                profilePicture = childdata.profilePicture,
                createdAt = DateTime.Now,
            });
            _context.SaveChanges();
            var userdata = from user in _context.Users
                           where user.email == childdata.email
                           select new
                           {
                               user.userID,
                               user.email,
                               user.password,
                               user.gender,
                               user.country,
                               user.userType,
                               user.profilePicture,
                               guardianID = childdata.guardianID,
                           };

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                message = "Guardian login successful"
            });
        }
    }
}
