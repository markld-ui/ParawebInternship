#region Заголовок файла

/// <summary>
/// Файл: AuthController.cs
/// Контроллер для обработки запросов, связанных с аутентификацией и регистрацией пользователей.
/// Предоставляет методы для регистрации новых пользователей и аутентификации существующих.
/// </summary>

#endregion

#region Пространства имен

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LandingAPI.Models;
using LandingAPI.DTO;
using LandingAPI.Services.Auth;
using LandingAPI.Interfaces.Auth;
using LandingAPI.Interfaces.Repositories;

#endregion

namespace LandingAPI.Controllers
{
    #region Класс AuthController

    /// <summary>
    /// Контроллер для управления аутентификацией и регистрацией пользователей.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Поля и свойства

        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthController"/>.
        /// </summary>
        /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
        /// <param name="jwtService">Сервис для генерации JWT-токенов.</param>
        /// <param name="passwordHasher">Сервис для хеширования и проверки паролей.</param>
        public AuthController(
            IUserRepository userRepository,
            JwtService jwtService,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        #endregion

        #region Методы

        #region Register

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="model">Модель данных для регистрации, содержащая имя пользователя, email и пароль.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с сообщением об успешной регистрации.
        /// </returns>
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
            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        #endregion

        #region Login

        /// <summary>
        /// Аутентифицирует пользователя и возвращает JWT-токен.
        /// </summary>
        /// <param name="model">Модель данных для входа, содержащая email и пароль.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 401 Unauthorized, если пользователь не найден или пароль неверен.
        /// - 200 OK с JWT-токеном в случае успешной аутентификации.
        /// </returns>
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

        #endregion

        #endregion
    }

    #endregion
}