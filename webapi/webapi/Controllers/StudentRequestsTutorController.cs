using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models;

namespace webapi.Controllers
{
    public class StudentRequestsTutorController : ApiController
    {


        onlineQuranTutorEntities4 _context = new onlineQuranTutorEntities4();

        [HttpPost]
        public HttpResponseMessage studentSendRequestToTutor(StudentRequestsTutorDTO requestdata)
        {


            if (requestdata.tutorid <= 0 || requestdata.studentid <= 0 || requestdata.subjectid <= 0)
            {
                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "Invalid Input"
                });
            }
            var check = _context.Students.Where(x => x.studentID == requestdata.studentid
                      && x.Subject.subjectID == requestdata.subjectid).FirstOrDefault();

            if (check == null)
            {
                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "No student found with this student ID and subject ID."
                });
            }

            _context.StudentTutorRequests.Add(new StudentTutorRequest()
            {
                Student = _context.Students.Where(s => s.studentID == requestdata.studentid).FirstOrDefault(),
                Tutor = _context.Tutors.Where(t => t.tutorID == requestdata.tutorid).FirstOrDefault(),
                Subject = _context.Subjects.Where(sub => sub.subjectID == requestdata.subjectid).FirstOrDefault(),
                status = "pending",
                createdAt = System.DateTime.Now,
                updatedAt = System.DateTime.Now
            });
            _context.SaveChanges();
            return Request.CreateResponse(new
            {
                statusCode = HttpStatusCode.OK,
                message = "Request Send Successfully"
            });
        }

        [HttpPost]
        public HttpResponseMessage CreateClasses(AcceptRequestFromTutorDTO request)
        {
            try
            {
                // -------------------------
                // 1. Validate Student
                // -------------------------
                var student = _context.Students.Find(request.studentID);
                if (student == null)
                    return Request.CreateResponse(new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Student Not Found",
                    });

                // Subject match validation
                if (student.Subject.subjectID != request.subjectID)
                    return Request.CreateResponse(new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Student is not assigned to this subject",
                    });

                // -------------------------
                // 2. Validate Tutor
                // -------------------------
                var tutor = _context.Tutors.Find(request.tutorID);
                if (tutor == null)
                    return Request.CreateResponse(new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Tutor Not Found",
                    });


                // Tutor must teach the subject
                bool tutorTeaches = _context.TutorSubjects
                    .Any(x => x.Tutor.tutorID == request.tutorID &&
                              x.Subject.subjectID == request.subjectID);

                if (!tutorTeaches)
                    return Request.CreateResponse(new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Tutor does not teach this subject",
                    });


                // -------------------------
                // 3. Validate LessonPlan
                // -------------------------
                var lessonPlan = _context.LessonPlans.Where(req => req.lessonPlanID == req.lessonPlanID).FirstOrDefault();
                if (lessonPlan == null)
                    return Request.CreateResponse(new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Lesson plan not found",
                    });


                // -------------------------
                // 4. Get Lessons for this subject + lessonplan
                // -------------------------
                var lessons = _context.Lessons
                    .Where(l => l.LessonPlan.lessonPlanID == request.lessonplanID &&
                                l.Subject.subjectID == request.subjectID)
                    .ToList();

                if (!lessons.Any())
                    return Request.CreateResponse(new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "No lessons found for this subject under this lesson plan",
                    });


                // -------------------------
                // 5. Check slot conflict (same day cannot have multiple slots)
                // -------------------------
                bool slotAlreadyBooked = _context.Classes.Any(c =>
                    c.Tutor.tutorID == request.tutorID &&
                    c.Day.dayID == request.dayID &&
                    c.Slot.slotID == request.slotID
                );

                if (slotAlreadyBooked)
                    return Request.CreateResponse(new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "This slot is already booked on this day",
                    });


                // -------------------------
                // 6. Create Classes for each Lesson
                // -------------------------
                List<ClassDTO> classDTOs = new List<ClassDTO>();
                foreach (var lesson in lessons)
                {
                    var newClass = new ClassDTO()
                    {
                        studentID = request.studentID,
                        tutorID = request.tutorID,
                        slotID = request.slotID,
                        dayID = request.dayID,
                        lessonplanID = request.lessonplanID,
                        studentRequestTutorID = request.studentRequestTutorID,
                        subjectID = request.subjectID,
                        status = "scheduled",
                        corrections = 0,
                        classDate = request.classDate,
                        createdAt = DateTime.Now
                    };
                    classDTOs.Add(newClass);
                    //_context.Classes.Add(new Class()
                    //{

                    //});
                }

                //_context.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    message = "Classes created successfully for all lessons.",
                    classes = classDTOs,
                    totalClasses = lessons.Count
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    message = "An error occurred.",
                    error = ex.Message
                });
            }
        }

    }
}
