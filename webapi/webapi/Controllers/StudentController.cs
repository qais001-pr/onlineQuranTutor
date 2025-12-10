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

        onlineQuranTutorEntities3 _context = new onlineQuranTutorEntities3();
        /// <summary>
        /// Add Child Subjects for Student
        /// After Registered from the signup screen the student can add subjects with preferred teachers
        /// </summary>
        /// <param name="childSubject"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage addChildSubjects(StudentSubject childSubject)
        {
            if (childSubject.subjectid <= 0 || string.IsNullOrWhiteSpace(childSubject.preffered_teachers))
            {
                return Request.CreateResponse(new
                {
                    statusCode = 400,
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
                return Request.CreateResponse(new
                {
                    statusCode = 200,
                    message = "Data Saved"
                });

            }
        }


        /// <summary>
        /// After Registered from the signup screen the student can add available slots
        /// </summary>
        /// <param name="studentSlots"></param>
        /// <returns></returns>




        [HttpPost]
        public HttpResponseMessage addStudentSlots(StudentSlots studentSlots)
        {
            if (studentSlots.studentid <= 0 || studentSlots.slotid <= 0 || studentSlots.dayid <= 0)
            {
                return Request.CreateResponse(new
                {
                    statusCode = 400,
                    message = "Invalid Data"
                });
            }


            var student = _context.StudentSlots.FirstOrDefault(s => s.Student.studentID == studentSlots.studentid && s.Day.dayID == studentSlots.dayid && s.Slot.slotID == studentSlots.slotid);


            if (student != null)
            {
                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.OK,
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

            return Request.CreateResponse(new
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
                return Request.CreateResponse(new
                {
                    statusCode = 400,
                    message = "Invalid Data"
                });
            }

            var data =
                    (from s in _context.Students
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
                         tutorID = t.tutorID,
                         name = u.name,
                         about = t.about,
                         status = ts.status
                     })
                    .Distinct()
                    .ToList();


            return Request.CreateResponse(new
            {
                statusCode = 200,
                data = data,
                message = "Data"
            });

        }




    }
}
