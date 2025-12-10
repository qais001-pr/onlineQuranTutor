using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models;
using System.Linq;

namespace webapi.Controllers
{
    public class TutorController : ApiController
    {
        onlineQuranTutorEntities3 _context = new onlineQuranTutorEntities3();

        // ---------------------------------------------------------
        // ADD TUTOR SLOT
        // ---------------------------------------------------------
        [HttpPost]
        public HttpResponseMessage addTutorSlots(TutorSlots tutorSlot)
        {
            // Input validation
            if (tutorSlot.tutorID <= 0 || tutorSlot.slotid <= 0 || tutorSlot.dayid <= 0 ||
                string.IsNullOrWhiteSpace(tutorSlot.status))
            {
                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "Invalid input data"
                });
            }

            // Check if slot already exists
            var tutor = _context.TutorSlots.Where(x =>
                x.Tutor.tutorID == tutorSlot.tutorID &&
                x.Slot.slotID == tutorSlot.slotid &&
                x.Day.dayID == tutorSlot.dayid
            ).FirstOrDefault();

            if (tutor != null)
            {
                return Request.CreateResponse(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = $"A slot already exists for Tutor {tutorSlot.tutorID} on day {tutorSlot.dayid}."
                });
            }

            // Add new slot
            _context.TutorSlots.Add(new TutorSlot()
            {
                Tutor = _context.Tutors.Find(tutorSlot.tutorID),
                Day = _context.Days.Find(tutorSlot.dayid),
                Slot = _context.Slots.Find(tutorSlot.slotid),
                status = "available"
            });

            _context.SaveChanges();

            return Request.CreateResponse(new
            {
                statusCode = HttpStatusCode.OK,
                message = "Tutor Slots added Successfully"
            });
        }

        // ---------------------------------------------------------
        // GET TUTOR DETAILS
        // ---------------------------------------------------------
        [HttpGet]
        public HttpResponseMessage getTutorDetails(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    message = "Invalid tutor ID"
                });
            }

            var tutor = (from t in _context.Tutors
                         where t.tutorID == id
                         select new
                         {
                             t.tutorID,
                             t.about,
                             userID = t.User.userID,
                             t.User.name,
                             t.User.email,
                             t.User.country,
                             t.User.city,
                             t.User.timezone,

                             subjects = (from ts in _context.TutorSubjects
                                         where ts.Tutor.tutorID == t.tutorID
                                         select new
                                         {
                                             ts.Subject.subjectID,
                                             ts.Subject.subjectName
                                         }),
                             ratings = (from c in _context.Classes
                                        join r in _context.Reviews on c.ClassID equals r.Class.ClassID
                                        where c.Tutor.tutorID == t.tutorID
                                        group r by c.Tutor.tutorID into g
                                        select new
                                        {
                                            AverageRating = g.Average(x => x.Rating),
                                            ReviewCount = g.Count()
                                        }).FirstOrDefault()
                         })
            .FirstOrDefault();


            if (tutor == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new
                {
                    message = "Tutor not found"
                });
            }

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                statusCode = HttpStatusCode.OK,
                data = tutor,
                message = "Data collected successfully"
            });
        }
    }
}
