using LandingAPI.Interfaces;
using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using LandingAPI.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<IActionResult> GetUsersAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = await _userRepository.GetUsersAsync();
            if (users == null)
                return NotFound();

            var usersDtos = _mapper.Map<List<UserDTO>>(users);
            return Ok(usersDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            if (!await _userRepository.UserExistsByIdAsync(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var users = await _userRepository.GetUserByIdAsync(id);
            var usersDtos = _mapper.Map<UserDTO>(users);
            return Ok(usersDtos);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserAsync(string username)
        {
            if (!await _userRepository.UserExistsByNameAsync(username))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var users = await _userRepository.GetUserByNameAsync(username);
            var usersDtos = _mapper.Map<UserDTO>(users);
            return Ok(usersDtos);
        }

        [HttpGet("{id}/News")]
        [ProducesResponseType(typeof(IEnumerable<NewsDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNewsByUserAsync(int id)
        {
            if (!await _userRepository.UserExistsByIdAsync(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var users = await _userRepository.GetNewsByUserIdAsync(id);
            var newsDtos = _mapper.Map<List<NewsDTO>>(users);
            return Ok(newsDtos);
        }
    }
}
