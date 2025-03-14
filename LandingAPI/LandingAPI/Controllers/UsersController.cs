using LandingAPI.Interfaces;
using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using LandingAPI.DTO;
using AutoMapper;

namespace LandingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDTO>))]
        public IActionResult GetUsers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = _mapper.Map<List<UserDTO>>(_userRepository.GetUsers());
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int id)
        {
            if (!_userRepository.UserExistsById(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<UserDTO>(_userRepository.GetUserById(id));
            return Ok(user);
        }

        [HttpGet("search")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(string username)
        {
            if (!_userRepository.UserExistsByName(username))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<UserDTO>(_userRepository.GetUserByName(username));
            return Ok(user);
        }

        [HttpGet("{id}/News")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NewsDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetNewsByUser(int id)
        {
            if (!_userRepository.UserExistsById(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var news = _mapper.Map<List<NewsDTO>>(_userRepository.GetNewsByUserId(id));
            return Ok(news);
        }
    }
}
