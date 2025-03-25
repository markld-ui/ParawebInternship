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
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using LandingAPI.Interfaces.Auth;
using LandingAPI.DTO.News;
using LandingAPI.DTO.Users;
using LandingAPI.Helper;

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
        private readonly IPasswordHasher _passwordHasher;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UsersController"/>.
        /// </summary>
        /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
        /// <param name="mapper">Объект для маппинга данных между моделями и DTO.</param>
        public UsersController(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
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
        /// - 200 OK с списком пользователей в формате <see cref="UserShortDTO"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<UserShortDTO>), 200)]
        public async Task<IActionResult> GetUsersAsync(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string sort = "UserId",
            [FromQuery] bool asc = true)
        {
            var (users, totalCount) = await _userRepository.GetUsersAsync(page, size, sort, asc);
            if (users == null || !users.Any())
                return NotFound("Пользователи не найдены");

            var usersDtos = users.Select(u => new UserShortDTO
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                MainRole = u.UserRoles.FirstOrDefault()?.Role.Name ?? "Без роли"
            }).ToList();

            return Ok(new PagedResponse<UserShortDTO>
            {
                Data = usersDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = size
            });
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
        /// - 200 OK с данными пользователя в формате <see cref="UserDetailsDTO"/>.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDetailsDTO), 200)]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("Пользователь не найден");

            return Ok(new UserDetailsDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Roles = user.UserRoles.Select(ur => new RoleDTO
                {
                    RoleId = ur.RoleId,
                    Name = ur.Role.Name
                }).ToList()
            });
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

        #region UpdateUser

        /// <summary>
        /// Обновляет данные пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя, данные которого нужно обновить.</param>
        /// <param name="model">Модель данных для обновления пользователя, содержащая новое имя пользователя, email и пароль.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если пользователь с указанным идентификатором не найден.
        /// - 200 OK с обновленными данными пользователя.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDetailsDTO>> UpdateUser(int id, [FromBody] UpdateUserDTO dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("Пользователь не найден");

            if (!string.IsNullOrEmpty(dto.Username) && dto.Username != user.Username)
            {
                if (await _userRepository.UserExistsByNameAsync(dto.Username))
                    return BadRequest("Пользователь с таким именем уже существует");

                user.Username = dto.Username;
            }

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                if (await _userRepository.UserExistsByEmailAsync(dto.Email))
                    return BadRequest("Пользователь с таким email уже существует");

                user.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = _passwordHasher.Generate(dto.Password);

            if (dto.RoleId.HasValue)
            {
                user.UserRoles.Clear();
                user.UserRoles.Add(new UserRole { RoleId = dto.RoleId.Value });
            }

            await _userRepository.UpdateUserAsync(user);
            return Ok(await MapToDetailsDTO(user));
        }

        #endregion

        #region DeleteUser

        /// <summary>
        /// Удаляет пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя, которого нужно удалить.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если пользователь с указанным идентификатором не найден.
        /// - 204 NoContent, если пользователь успешно удален.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userRepository.DeleteUserAsync(user);
            return NoContent();
        }

        #endregion

        #region MapToDetailsDTO

        /// <summary>
        /// Преобразует объект <see cref="User "/> в объект <see cref="User DetailsDTO"/>.
        /// </summary>
        /// <param name="user">Объект пользователя, который нужно преобразовать.</param>
        /// <returns>
        /// Возвращает объект <see cref="User DetailsDTO"/>, содержащий детали пользователя.
        /// </returns>
        /// <remarks>
        /// Метод создает DTO (Data Transfer Object) для передачи данных о пользователе,
        /// включая информацию о ролях пользователя.
        /// </remarks>
        private async Task<UserDetailsDTO> MapToDetailsDTO(User user)
        {
            return new UserDetailsDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Roles = user.UserRoles.Select(ur => new RoleDTO
                {
                    RoleId = ur.RoleId,
                    Name = ur.Role.Name
                }).ToList()
            };
        }

        #endregion

        #endregion
    }

    #endregion
}