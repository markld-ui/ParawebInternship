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
using LandingAPI.DTO.Roles;
using LandingAPI.Helper;

#endregion

namespace LandingAPI.Controllers
{
    #region Класс UsersController

    /// <summary>
    /// Контроллер для управления пользователями.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("99.0")]
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
        /// <param name="passwordHasher">Объект для работы с паролем</param>
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
        /// - 200 OK с списком пользователей в формате <see cref="User ShortDTO"/>.
        /// - 404 NotFound, если пользователи не найдены.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// Метод возвращает список пользователей с поддержкой пагинации и сортировки.
        /// 
        /// ### Параметры:
        /// - **page**: Номер страницы для пагинации (по умолчанию 1).
        /// - **size**: Количество пользователей на странице (по умолчанию 10).
        /// - **sort**: Поле для сортировки (по умолчанию "User Id").
        /// - **asc**: Указывает, будет ли сортировка по возрастанию (по умолчанию true).
        /// 
        /// ### Пример запроса:
        /// GET /api/users?page=1&size=10&sort=Username&asc=true
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// {
        ///   "data": [
        ///     {
        ///       "userId": 1,
        ///       "username": "user1",
        ///       "email": "user1@example.com",
        ///       "createdAt": "2023-01-01T12:00:00Z",
        ///       "mainRole": "Администратор"
        ///     },
        ///     {
        ///       "userId": 2,
        ///       "username": "user2",
        ///       "email": "user2@example.com",
        ///       "createdAt": "2023-01-02T12:00:00Z",
        ///       "mainRole": "Пользователь"
        ///     }
        ///   ],
        ///   "totalCount": 50,
        ///   "page": 1,
        ///   "pageSize": 10
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (пользователи не найдены):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Пользователи не найдены"
        /// }
        /// ```
        /// </remarks>
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
        /// - 200 OK с данными пользователя в формате <see cref="User DetailsDTO"/>.
        /// - 404 NotFound, если пользователь с указанным идентификатором не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// Метод возвращает детали пользователя по его уникальному идентификатору.
        /// 
        /// ### Пример запроса:
        /// GET /api/users/1
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// {
        ///   "userId": 1,
        ///   "username": "user1",
        ///   "email": "user1@example.com",
        ///   "createdAt": "2023-01-01T12:00:00Z",
        ///   "roles": [
        ///     {
        ///       "roleId": 1,
        ///       "name": "Admin"
        ///     },
        ///     {
        ///       "roleId": 2,
        ///       "name": "User"
        ///     }
        ///   ]
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (пользователь не найден):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Пользователь не найден"
        /// }
        /// ```
        /// </remarks>
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
        /// - 200 OK с данными пользователя в формате <see cref="User DTO"/>.
        /// - 404 NotFound, если пользователь с указанным именем не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// Метод возвращает информацию о пользователе по его имени.
        /// 
        /// ### Пример запроса:
        /// GET /api/users/search?username=user1
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// {
        ///   "userId": 1,
        ///   "username": "user1",
        ///   "email": "user1@example.com",
        ///   "createdAt": "2023-01-01T12:00:00Z",
        ///   "roles": [
        ///     {
        ///       "roleId": 1,
        ///       "name": "Администратор"
        ///     }
        ///   ]
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (пользователь не найден):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Пользователь не найден"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// - **HTTP/1.1 400 Bad Request**
        /// ```json
        /// {
        ///   "error": "Некорректный запрос"
        /// }
        /// ```
        /// </remarks>
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
        /// - 200 OK с списком новостей в формате <see cref="NewsDTO"/>.
        /// - 404 NotFound, если пользователь с указанным идентификатором не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// Метод возвращает все новости, связанные с пользователем по его уникальному идентификатору.
        /// 
        /// ### Пример запроса:
        /// GET /api/users/1/News
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// [
        ///   {
        ///     "newsId": 1,
        ///     "title": "Заголовок новости 1",
        ///     "content": "Содержимое новости 1",
        ///     "createdAt": "2023-01-01T12:00:00Z"
        ///   },
        ///   {
        ///     "newsId": 2,
        ///     "title": "Заголовок новости 2",
        ///     "content": "Содержимое новости 2",
        ///     "createdAt": "2023-01-02T12:00:00Z"
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Пример ошибки (пользователь не найден):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Пользователь не найден"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// - **HTTP/1.1 400 Bad Request**
        /// ```json
        /// {
        ///   "error": "Некорректный запрос"
        /// }
        /// ```
        /// </remarks>
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
        /// - 200 OK с обновленными данными пользователя в формате <see cref="User DetailsDTO"/>.
        /// - 404 NotFound, если пользователь с указанным идентификатором не найден.
        /// - 400 BadRequest, если модель данных невалидна или пользователь с таким именем/email уже существует.
        /// </returns>
        /// <remarks>
        /// Метод обновляет информацию о пользователе, включая имя, email и пароль.
        /// 
        /// ### Пример запроса:
        /// PUT /api/users/1
        /// 
        /// ```json
        /// {
        ///   "username": "newUsername",
        ///   "email": "newemail@example.com",
        ///   "password": "newPassword",
        ///   "roleId": 2
        /// }
        /// ```
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// {
        ///   "userId": 1,
        ///   "username": "newUsername",
        ///   "email": "newemail@example.com",
        ///   "createdAt": "2023-01-01T12:00:00Z",
        ///   "roles": [
        ///     {
        ///       "roleId": 2,
        ///       "name": "Пользователь"
        ///     }
        ///   ]
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (пользователь не найден):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Пользователь не найден"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (пользователь с таким именем/email уже существует):
        /// - **HTTP/1.1 400 Bad Request**
        /// ```json
        /// {
        ///   "error": "Пользователь с таким именем уже существует"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// - **HTTP/1.1 400 Bad Request**
        /// ```json
        /// {
        ///   "error": "Некорректный запрос"
        /// }
        /// ```
        /// </remarks>
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
        /// - 204 NoContent, если пользователь успешно удален.
        /// - 404 NotFound, если пользователь с указанным идентификатором не найден.
        /// </returns>
        /// <remarks>
        /// Метод удаляет пользователя из системы по его уникальному идентификатору.
        /// 
        /// ### Пример запроса:
        /// DELETE /api/users/1
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 204 No Content**
        /// 
        /// ### Пример ошибки (пользователь не найден):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Пользователь не найден"
        /// }
        /// ```
        /// </remarks>
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
        /// 
        /// ### Пример использования:
        /// ```csharp
        /// User user = await _userRepository.GetUser ByIdAsync(userId);
        /// UserDetailsDTO userDetails = await MapToDetailsDTO(user);
        /// ```
        /// 
        /// ### Структура возвращаемого объекта:
        /// ```json
        /// {
        ///   "userId": 1,
        ///   "username": "exampleUser ",
        ///   "email": "user@example.com",
        ///   "createdAt": "2023-01-01T12:00:00Z",
        ///   "roles": [
        ///     {
        ///       "roleId": 1,
        ///       "name": "Администратор"
        ///     },
        ///     {
        ///       "roleId": 2,
        ///       "name": "Пользователь"
        ///     }
        ///   ]
        /// }
        /// ```
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