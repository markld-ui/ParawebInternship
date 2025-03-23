#region Заголовок файла

/// <summary>
/// Файл: UsersController.cs
/// Контроллер для управления пользователями.
/// Предоставляет методы для получения списка пользователей, поиска пользователя по идентификатору или имени, а также получения новостей, связанных с конкретным пользователем.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using LandingAPI.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

#endregion

namespace LandingAPI.Controllers
{
    #region Класс UsersController

    /// <summary>
    /// Контроллер для управления пользователями.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        #region Поля и свойства

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UsersController"/>.
        /// </summary>
        /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
        /// <param name="mapper">Объект для маппинга данных между моделями и DTO.</param>
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        #endregion

        #region Методы

        #region GetUsersAsync

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если пользователи не найдены.
        /// - 200 OK с списком пользователей в формате <see cref="UserDTO"/>.
        /// </returns>
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

        #endregion

        #region GetUserAsync (по идентификатору)

        /// <summary>
        /// Получает пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если пользователь с указанным идентификатором не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с данными пользователя в формате <see cref="UserDTO"/>.
        /// </returns>
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

        #endregion

        #region GetUserAsync (по имени)

        /// <summary>
        /// Получает пользователя по его имени.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если пользователь с указанным именем не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с данными пользователя в формате <see cref="UserDTO"/>.
        /// </returns>
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

        #endregion

        #region GetNewsByUserAsync

        /// <summary>
        /// Получает список новостей, связанных с конкретным пользователем.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если пользователь с указанным идентификатором не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с списком новостей в формате <see cref="NewsDTO"/>.
        /// </returns>
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

        #endregion

        #endregion
    }

    #endregion
}