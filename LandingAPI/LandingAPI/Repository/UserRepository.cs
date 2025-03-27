#region Заголовок файла

/// <summary>
/// Файл: UserRepository.cs
/// Реализация репозитория для работы с сущностью <see cref="User"/>.
/// Предоставляет методы для доступа к данным о пользователях в базе данных, включая их новости и проверку существования.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Data;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace LandingAPI.Repository
{
    #region Класс UserRepository

    /// <summary>
    /// Реализация репозитория для работы с сущностью <see cref="User"/>.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        #region Поля

        private readonly DataContext _context;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        #endregion

        #region Методы

        #region GetUsersAsync
        /// <summary>
        /// Получает список всех пользователей, отсортированных по указанному полю.
        /// </summary>
        /// <param name="pageNumber">Номер страницы для получения (по умолчанию 1).</param>
        /// <param name="pageSize">Количество пользователей на странице (по умолчанию 10).</param>
        /// <param name="sortField">Поле, по которому будет производиться сортировка (по умолчанию "User Id").</param>
        /// <param name="ascending">Определяет порядок сортировки: по возрастанию или убыванию (по умолчанию true).</param>
        /// <returns>Кортеж, содержащий коллекцию пользователей и общее количество пользователей.</returns>
        public async Task<(ICollection<User> Users, int TotalCount)> GetUsersAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortField = "UserId",
            bool ascending = true)
        {
            var query = _context.Users.AsQueryable();

            query = sortField switch
            {
                "Email" => ascending ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
                "Username" => ascending ? query.OrderBy(u => u.Username) : query.OrderByDescending(u => u.Username),
                _ => ascending ? query.OrderBy(u => u.UserId) : query.OrderByDescending(u => u.UserId)
            };

            var totalCount = await query.CountAsync();
            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalCount);
        }
        #endregion

        #region GetUserByIdAsync
        /// <summary>
        /// Получает пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Пользователь, если найден, иначе <c>null</c>.</returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.Where(n => n.UserId == id).FirstOrDefaultAsync();
        }
        #endregion

        #region GetUserByNameAsync
        /// <summary>
        /// Получает пользователя по его имени.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>Пользователь, если найден, иначе <c>null</c>.</returns>
        public async Task<User> GetUserByNameAsync(string username)
        {
            return await _context.Users.Where(n => n.Username == username).FirstOrDefaultAsync();
        }
        #endregion

        #region GetNewsByUserIdAsync
        /// <summary>
        /// Получает список новостей, созданных определенным пользователем.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Коллекция новостей, созданных пользователем.</returns>
        public async Task<ICollection<News>> GetNewsByUserIdAsync(int userId)
        {
            return await _context.News.Where(n => n.CreatedById == userId).ToListAsync();
        }
        #endregion

        #region UserExistsByIdAsync
        /// <summary>
        /// Проверяет существование пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>
        /// <c>true</c>, если пользователь с указанным идентификатором существует, иначе <c>false</c>.
        /// </returns>
        public async Task<bool> UserExistsByIdAsync(int id)
        {
            return await _context.Users.AnyAsync(n => n.UserId == id);
        }
        #endregion

        #region UserExistsByNameAsync
        /// <summary>
        /// Проверяет существование пользователя по его имени.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>
        /// <c>true</c>, если пользователь с указанным именем существует, иначе <c>false</c>.
        /// </returns>
        public async Task<bool> UserExistsByNameAsync(string username)
        {
            return await _context.Users.AnyAsync(n => n.Username == username);
        }
        #endregion

        #region GetUserByEmailAsync
        /// <summary>
        /// Получает пользователя по его электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <returns>Пользователь, если найден, иначе <c>null</c>.</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(u => u.Email == email);
        }
        #endregion

        #region UserExistsByEmailAsync
        /// <summary>
        /// Проверяет существование пользователя по его электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <returns>
        /// <c>true</c>, если пользователь с указанной электронной почтой существует, иначе <c>false</c>.
        /// </returns>
        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        #endregion

        #region AddUserAsync
        /// <summary>
        /// Добавляет нового пользователя в базу данных.
        /// </summary>
        /// <param name="user">Пользователь для добавления.</param>
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region UpdateUserAsync
        /// <summary>
        /// Обновляет данные пользователя в базе данных.
        /// </summary>
        /// <param name="user">Пользователь для обновления.</param>
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DeleteUserAsync
        /// <summary>
        /// Удаляет пользователя из базы данных.
        /// </summary>
        /// <param name="user">Пользователь для удаления.</param>
        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region AssignRoleAsync
        /// <summary>
        /// Назначает роль пользователю.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="roleId">Идентификатор роли.</param>
        /// <exception cref="Exception">
        /// Возникает, если пользователь или роль не найдены, либо если роль уже назначена пользователю.
        /// </exception>
        public async Task AssignRoleAsync(int userId, int roleId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new Exception("Пользователь не найден.");

            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
                throw new Exception("Роль не найдена.");

            if (user.UserRoles.Any(ur => ur.RoleId == roleId))
                throw new Exception("Роль уже назначена пользователю.");

            user.UserRoles.Add(new UserRole { RoleId = roleId });
            await _context.SaveChangesAsync();
        }
        #endregion

        #region RemoveRoleAsync
        /// <summary>
        /// Удаляет роль у пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="roleId">Идентификатор роли.</param>
        /// <exception cref="Exception">
        /// Возникает, если пользователь не найден или роль не назначена пользователю.
        /// </exception>
        public async Task RemoveRoleAsync(int userId, int roleId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new Exception("Пользователь не найден.");

            var userRole = user.UserRoles.FirstOrDefault(ur => ur.RoleId == roleId);
            if (userRole == null)
                throw new Exception("Роль не назначена пользователю.");

            user.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
        #endregion

        #endregion
    }

    #endregion
}