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
using System.Security.Claims;

#endregion

namespace LandingAPI.Controllers
{
    #region Класс AuthController

    /// <summary>
    /// Контроллер для управления аутентификацией и регистрацией пользователей.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        #region Поля и свойства

        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthController> _logger;
        private const int _refreshTokenExpiryDays = 7;

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
            IPasswordHasher passwordHasher, 
            ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _logger = logger;
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
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _userRepository.UserExistsByEmailAsync(model.Email))
                    return BadRequest("Пользователь с таким email уже существует.");

                if (await _userRepository.UserExistsByNameAsync(model.Username))
                    return BadRequest("Пользователь с таким именем уже существует.");

                var refreshToken = _jwtService.GenerateRefreshToken();

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = _passwordHasher.Generate(model.Password),
                    CreatedAt = DateTime.UtcNow,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays)
                };

                await _userRepository.AddUserAsync(user);

                var token = _jwtService.GenerateToken(user);

                return Ok(new
                {
                    Token = token,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя.");
                return StatusCode(500, "Произошла ошибка при регистрации.");
            }
        }

        #endregion

        #region Login

        /// <summary>
        /// Аутентифицирует пользователя и возвращает JWT-токен.
        /// </summary>
        /// <param name="model">Модель данных для входа, содержащая email и пароль.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 500 Internal Server Error, если произошла ошибка в запросе
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
            {
                _logger.LogWarning("Неудачная попытка входа для email: {Email}", model.Email);
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            await _userRepository.UpdateUserAsync(user);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        #endregion

        #region Refresh

        /// <summary>
        /// Обновляет JWT-токен и refresh token.
        /// </summary>
        /// <param name="model">Модель данных, содержащая текущий JWT-токен и refresh token.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если токен невалиден или срок действия refresh token истек.
        /// - 200 OK с новым JWT-токеном и refresh token в случае успешного обновления.
        /// </returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(model.Token);
            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Неудачная попытка обновления токена для пользователя с ID: {UserId}", userId);
                return BadRequest("Invalid token");
            }

            var newToken = _jwtService.GenerateToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            await _userRepository.UpdateUserAsync(user);

            return Ok(new
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            });
        }
        #endregion

        #endregion
    }

    #endregion
}