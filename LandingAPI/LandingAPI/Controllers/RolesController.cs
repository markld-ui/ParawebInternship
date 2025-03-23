#region Заголовок файла

/// <summary>
/// Файл: RolesController.cs
/// Контроллер для управления ролями пользователей.
/// Предоставляет методы для назначения и удаления ролей, а также получения списка ролей пользователя.
/// Доступ к методам контроллера разрешен только пользователям с ролью "Admin".
/// </summary>

#endregion

using LandingAPI.DTO;
using LandingAPI.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingAPI.Controllers
{
    #region Класс RolesController
    /// <summary>
    /// Контроллер для управления ролями пользователей.
    /// Предоставляет методы для назначения и удаления ролей, а также получения списка ролей пользователя.
    /// Доступ к методам контроллера разрешен только пользователям с ролью "Admin".
    /// </summary>
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        #region Поля и свойства
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        #endregion

        #region Конструкторы
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RolesController"/>.
        /// </summary>
        /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
        /// <param name="roleRepository">Репозиторий для работы с ролями.</param>
        public RolesController(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        #endregion

        #region Методы

        #region AssignRole
        /// <summary>
        /// Назначает роль пользователю.
        /// </summary>
        /// <param name="model">Модель данных для назначения роли, содержащая идентификатор пользователя и идентификатор роли.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с сообщением об успешном назначении роли.
        /// - 400 BadRequest, если произошла ошибка при назначении роли.
        /// </returns>
        /// <remarks>
        /// Пример запроса:
        /// POST /api/roles/assign
        /// {
        ///     "userId": 1,
        ///     "roleId": 2
        /// }
        /// </remarks>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO model)
        {
            try
            {
                await _userRepository.AssignRoleAsync(model.UserId, model.RoleId);
                return Ok("Роль успешно назначена.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region RemoveRole
        /// <summary>
        /// Удаляет роль у пользователя.
        /// </summary>
        /// <param name="model">Модель данных для удаления роли, содержащая идентификатор пользователя и идентификатор роли.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с сообщением об успешном удалении роли.
        /// - 400 BadRequest, если произошла ошибка при удалении роли.
        /// </returns>
        /// <remarks>
        /// Пример запроса:
        /// POST /api/roles/remove
        /// {
        ///     "userId": 1,
        ///     "roleId": 2
        /// }
        /// </remarks>
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDTO model)
        {
            try
            {
                await _userRepository.RemoveRoleAsync(model.UserId, model.RoleId);
                return Ok("Роль успешно удалена.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetUserRoles
        /// <summary>
        /// Получает список ролей пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если пользователь с указанным идентификатором не найден.
        /// - 200 OK с списком ролей пользователя.
        /// </returns>
        /// <remarks>
        /// Пример запроса:
        /// GET /api/roles/1/roles
        /// </remarks>
        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден.");

            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            return Ok(roles);
        }
        #endregion
        #endregion
    }
    #endregion
}