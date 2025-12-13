using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models;
namespace webapi.Controllers
{
    public class ReviewController : ApiController
    {

        onlineQuranTutorEntities4 _context = new onlineQuranTutorEntities4();
        [HttpPost]
        public HttpResponseMessage submitReview(ReviewDTO review)
        {
            if (review.classID <= 0 || string.IsNullOrWhiteSpace(review.comment) || review.rating <= 0 || review.rating > 5)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid review data.");
            }
            _context.Reviews.Add(new Review
            {
                Class = _context.Classes.Where(r=>r.classID == review.classID).FirstOrDefault(),
                Comment = review.comment,
                Rating = review.rating,
                CreatedAt = DateTime.Now,
            });
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Review Submit Successfully");
        }
    }
}
