using LandingAPI.Interfaces;
using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LandingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = _userRepository.GetUsers();
            return Ok(users);
        }
    }
}
