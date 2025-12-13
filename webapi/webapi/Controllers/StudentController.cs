using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models;
namespace webapi.Controllers
{
    public class StudentController : ApiController
    {

        onlineQuranTutorEntities4 _context = new onlineQuranTutorEntities4();
        [HttpPost]
        public HttpResponseMessage addChildSubjects(StudentSubject childSubject)
        {
            if (childSubject.subjectid <= 0 || string.IsNullOrWhiteSpace(childSubject.preffered_teachers))
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, new
                    {
                        message = "Invalid Data"
                    });
            }

            else
            {

                _context.Students.Add(new Student()
                {
                    User = _context.Users.Where(u => u.userID == childSubject.userid).FirstOrDefault(),
                    preferred_teacher = childSubject.preffered_teachers,
                    Subject = _context.Subjects.Where(s => s.subjectID == childSubject.subjectid).FirstOrDefault(),
                    createdAt = DateTime.Now,
                });
                _context.SaveChanges();
                return Request.CreateResponse(
                    HttpStatusCode.OK, new
                    {
                        message = "Data Saved"
                    });

            }
        }
        [HttpPost]
        public HttpResponseMessage addStudentSlots(StudentSlots studentSlots)
        {
            if (studentSlots.studentid <= 0 || studentSlots.slotid <= 0 || studentSlots.dayid <= 0)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, new
                    {
                        message = "Invalid Data"
                    });
            }


            var student = _context.StudentSlots.FirstOrDefault(s => s.Student.studentID == studentSlots.studentid && s.Day.dayID == studentSlots.dayid && s.Slot.slotID == studentSlots.slotid);


            if (student != null)
            {
                return Request.CreateResponse(
                    HttpStatusCode.OK, new
                    {
                        message = $"A slot already exists for student {studentSlots.studentid} on day {studentSlots.dayid}."

                    });
            }

            _context.StudentSlots.Add(new StudentSlot()
            {
                Student = _context.Students.FirstOrDefault(s => s.studentID == studentSlots.studentid),
                Slot = _context.Slots.FirstOrDefault(s => s.slotID == studentSlots.slotid),
                Day = _context.Days.FirstOrDefault(d => d.dayID == studentSlots.dayid)
            });

            _context.SaveChanges();

            return Request.CreateResponse(
                HttpStatusCode.OK, new
                {
                    statusCode = 200,
                    message = "Saved Successfully"
                });
        }

        [HttpGet]

        public HttpResponseMessage getAvailableTutors(int studentid)
        {
            if (studentid <= 0)
            {
                return Request.CreateResponse(
                    HttpStatusCode.OK, new
                    {
                        message = "Invalid Data"
                    });
            }

            var data = (from s in _context.Students
                        join sub in _context.TutorSubjects
                            on s.Subject.subjectID equals sub.Subject.subjectID
                        join t in _context.Tutors
                            on sub.Tutor.tutorID equals t.tutorID
                        join ts in _context.TutorSlots
                            on t.tutorID equals ts.Tutor.tutorID
                        join ss in _context.StudentSlots
                            on new { ts.Slot.slotID, ts.Day.dayID }
                            equals new { ss.Slot.slotID, ss.Day.dayID }
                        join d in _context.Days
                            on ts.Day.dayID equals d.dayID
                        join u in _context.Users
                            on t.User.userID equals u.userID
                        where s.studentID == studentid
                              && ts.status == "available"
                        select new
                        {
                            TutorID = t.tutorID,
                            Name = u.name,
                            About = t.about,
                            Status = ts.status,
                        })
             .Distinct()
             .ToList();



            return Request.CreateResponse(
                HttpStatusCode.OK, new
                {
                    data = data,
                    message = "Data"
                });

        }




    }
}
