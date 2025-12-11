using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models;
namespace webapi.Controllers
{
    public class UsersController : ApiController
    {
        onlineQuranTutorEntities4 _context = new onlineQuranTutorEntities4();
        [HttpPost]
        public HttpResponseMessage addUser(SignUpUser user)
        {
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
                    statusCode =HttpStatusCode.NotAcceptable,
                    message = "All fields are required"
                });
            }

            var checkUser = _context.Users.Where(u => u.email == user.email).FirstOrDefault();
            if (checkUser != null)
            {
                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.NotAcceptable,
                    message = $"User with email {user.email} already exists"
                });
            }

            byte[] pictureBytes;
            try
            {
                pictureBytes = Convert.FromBase64String(user.profile64String);
            }
            catch
            {
                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.Forbidden,
                    message = "Invalid Base64 image format"
                });
            }

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
            if (user.userType.Equals("Guardian", StringComparison.OrdinalIgnoreCase))
            {
                var us = _context.Users.FirstOrDefault(u => u.email == newUser.email);
                _context.Guardians.Add(new Guardian
                {
                    User = us
                });
                _context.SaveChanges();
                var registereduser = from u in _context.Users
                                     where u.email == newUser.email
                                     join g in _context.Guardians on u.userID equals g.User.userID
                                     select new
                                     {
                                         g.guardianID,
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
                    statusCode = HttpStatusCode.OK,
                    message = "Guardian added successfully",
                    data = registereduser,
                });
            }
            if (user.userType.Equals("Tutor", StringComparison.OrdinalIgnoreCase) &&
                user.tutorSubjects != null &&
                user.tutorSubjects.Count > 0)
            {
                var tutor = new Tutor
                {
                    User = newUser,
                    about = ""
                };

                _context.Tutors.Add(tutor);
                _context.SaveChanges();
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
                var registereduser = from u in _context.Users
                                     where u.email == newUser.email
                                     join t in _context.Tutors on u.userID equals t.User.userID
                                     select new
                                     {
                                         u.userID,
                                         t.tutorID,
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
                    statusCode = HttpStatusCode.OK,
                    message = "Tutor added successfully",
                    data = registereduser,
                });
            }
            return Request.CreateResponse(new
            {
                statusCode = HttpStatusCode.OK,
                message = "User added successfully"
            });
        }

        [HttpPost]
        public HttpResponseMessage loginUser(Auth auth)
        {
            if (auth == null)
            {
                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.BadRequest,
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
                statusCode = HttpStatusCode.OK,
                data = user,
                message = "Login successful"
            });
        }
    }
}


