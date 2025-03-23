using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LandingAPI.Models;
using LandingAPI.DTO;
using LandingAPI.Services.Auth;
using LandingAPI.Interfaces.Auth;
using LandingAPI.Interfaces.Repositories;

namespace LandingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(
            IUserRepository userRepository,
            JwtService jwtService,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = _passwordHasher.Generate(model.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddUserAsync(user);
            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null || !_passwordHasher.Verify(model.Password, user.PasswordHash))
                return Unauthorized();

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}