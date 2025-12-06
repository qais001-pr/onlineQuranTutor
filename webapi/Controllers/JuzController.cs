using Microsoft.AspNetCore.Mvc;
using onlineQuranTutor.Models;

namespace onlineQuranTutor.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class JuzController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JuzController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult GetAllJuz()
        {
            try
            {
                var juzList = _context.Juzs.ToList();

                return Ok(juzList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult TestConnection()
        {
            try
            {
                var juzCount = _context.Juzs.Count();
                var surahCount = _context.Surahs.Count();
                var quranCount = _context.Qurans.Count();
                return Ok(new
                {
                    status = "Connected",
                    JuzCount = juzCount,
                    SurahCount = surahCount,
                    QuranCount = quranCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "Failed",
                    error = ex.Message
                });
            }
        }
        [HttpGet]
        public ActionResult GetJuzSurahQuran()
        {
            try
            {
                var data = from j in _context.Juzs
                           join s in _context.Surahs on j.SurahId equals s.Id
                           join q in _context.Qurans on s.Id equals q.SuraId
                           where j.JuzId == 1 && s.Id == 1
                           select new { j.JuzId,s.Id,q.AyahText};
                if (data == null)
                    return NotFound(new { message = "No data found for the specified IDs." });

                return Ok(new { result = data, statusCode = 200 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}