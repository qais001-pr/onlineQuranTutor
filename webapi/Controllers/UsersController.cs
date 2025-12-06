using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onlineQuranTutor.Models;
namespace onlineQuranTutor.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getUsers()
        {
            var users = _context.Users.Count();
            return Ok(users);
        }

    }
}
