using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using webapi.Models;
namespace webapi.Controllers
{
    public class UsersController : ApiController
    {
        onlineQuranTutorEntities _context = new onlineQuranTutorEntities();
        [HttpPost]
        public HttpResponseMessage addUser(SignUpUser user)
        {
            // BASIC VALIDATION 
            if (user == null ||
                string.IsNullOrWhiteSpace(user.name) ||
                string.IsNullOrWhiteSpace(user.email) ||
                string.IsNullOrWhiteSpace(user.password) ||
                string.IsNullOrWhiteSpace(user.gender) ||
                user.dateOfBirth == DateTime.MinValue ||
                string.IsNullOrWhiteSpace(user.userType) ||
                string.IsNullOrWhiteSpace(user.city) ||
                string.IsNullOrWhiteSpace(user.timezone) ||
                string.IsNullOrWhiteSpace(user.profileType) ||
                string.IsNullOrWhiteSpace(user.country) ||
                string.IsNullOrWhiteSpace(user.profile64String))
            {
                return Request.CreateResponse(new
                {
                    statusCode = 400,
                    message = "All fields are required"
                });
            }

            // CHECK DUPLICATE USER 
            var checkUser = _context.Users.FirstOrDefault(u => u.email == user.email);
            if (checkUser != null)
            {
                return Request.CreateResponse(new
                {
                    statusCode = 409,
                    message = $"User with email {user.email} already exists"
                });
            }

            // CONVERT BASE64 IMAGE 
            byte[] pictureBytes;
            try
            {
                pictureBytes = Convert.FromBase64String(user.profile64String);
            }
            catch
            {
                return Request.CreateResponse(new
                {
                    statusCode = 400,
                    message = "Invalid Base64 image format"
                });
            }

            // CREATE USER 
            var newUser = new User
            {
                name = user.name,
                email = user.email,
                password = user.password,
                gender = user.gender,
                userType = user.userType,
                dateOfBirth = user.dateOfBirth,
                city = user.city,
                timezone = user.timezone,
                profilePicture = pictureBytes,
                country = user.country,
                pictureType = user.profileType,
                createdAt = DateTime.Now
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            if (user.userType.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                var u = _context.Users.FirstOrDefault(us => us.email == newUser.email);
                _context.Students.Add(new Student
                {
                    User = u
                });
                _context.SaveChanges();
            }
            // IF USER IS GUARDIAN, Create Guardian Record
            if (user.userType.Equals("Guardian", StringComparison.OrdinalIgnoreCase))
            {
                var u = _context.Users.FirstOrDefault(us => us.email == newUser.email);
                _context.Guardians.Add(new Guardian
                {
                    User = u
                });
                _context.SaveChanges();
            }
            if (user.userType.Equals("Tutor", StringComparison.OrdinalIgnoreCase) &&
                user.tutorSubjects != null &&
                user.tutorSubjects.Count > 0)
            {
                //  Create Tutor
                var tutor = new Tutor
                {
                    User = newUser,
                    about = ""
                };

                _context.Tutors.Add(tutor);
                _context.SaveChanges();

                // Add Tutor Subjects
                foreach (var subject in user.tutorSubjects)
                {
                    var subj = _context.Subjects.FirstOrDefault(s => s.subjectID == subject.subjectid);
                    if (subj != null)
                    {
                        _context.TutorSubjects.Add(new TutorSubject
                        {
                            Tutor = tutor,
                            Subject = subj
                        });
                    }
                }

                _context.SaveChanges();
            }

            var registereduser = from u in _context.Users
                                 where u.email == newUser.email
                                 select new
                                 {
                                     u.userID,
                                     u.name,
                                     u.gender,
                                     u.dateOfBirth,
                                     u.email,
                                     u.password,
                                     u.country,
                                     u.city,
                                     u.timezone,
                                     u.profilePicture,
                                     u.pictureType,
                                 };
            return Request.CreateResponse(new
            {
                statusCode = 200,
                message = "User added successfully",
                data = registereduser,
            });
        }









        [HttpPost]
        public HttpResponseMessage loginUser(Auth auth)
        {
            if (auth == null)
            {
                return Request.CreateResponse(new
                {
                    statusCode = 400,
                    message = "Invalid request"
                });
            }

            var user = from u in _context.Users
                       where u.email == auth.email && u.password == auth.password
                       select new
                       {
                           userid = u.userID,
                           name = u.name,
                           email = u.email,
                           password = u.password,
                           gender = u.gender,
                           userType = u.userType,
                           dateOfBirth = u.dateOfBirth,
                           city = u.city,
                           timezone = u.timezone,
                           profile64String = u.profilePicture,
                           imageType = u.pictureType,

                       };
            return Request.CreateResponse(new
            {
                statusCode = 200,
                data = user,
                message = "Login successful"
            });
        }
    }
}



