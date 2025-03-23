#region Заголовок файла

/// <summary>
/// Файл: IUserRepository.cs
/// Интерфейс репозитория для работы с сущностью <see cref="User"/>.
/// Определяет методы для работы с пользователями в базе данных, включая получение, проверку существования и добавление пользователей.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace LandingAPI.Interfaces.Repositories
{
    #region Интерфейс IUserRepository

    /// <summary>
    /// Интерфейс репозитория для работы с сущностью <see cref="User"/>.
    /// </summary>
    public interface IUserRepository
    {
        #region Методы

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <returns>Коллекция пользователей.</returns>
        Task<ICollection<User>> GetUsersAsync();

        /// <summary>
        /// Получает пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Пользователь, если найден, иначе <c>null</c>.</returns>
        Task<User> GetUserByIdAsync(int id);

        /// <summary>
        /// Получает пользователя по его имени.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>Пользователь, если найден, иначе <c>null</c>.</returns>
        Task<User> GetUserByNameAsync(string username);

        /// <summary>
        /// Получает пользователя по его электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <returns>Пользователь, если найден, иначе <c>null</c>.</returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Получает список новостей, созданных определенным пользователем.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Коллекция новостей, созданных пользователем.</returns>
        Task<ICollection<News>> GetNewsByUserIdAsync(int userId);

        /// <summary>
        /// Проверяет существование пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>
        /// <c>true</c>, если пользователь с указанным идентификатором существует, иначе <c>false</c>.
        /// </returns>
        Task<bool> UserExistsByIdAsync(int id);

        /// <summary>
        /// Проверяет существование пользователя по его имени.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>
        /// <c>true</c>, если пользователь с указанным именем существует, иначе <c>false</c>.
        /// </returns>
        Task<bool> UserExistsByNameAsync(string username);

        /// <summary>
        /// Проверяет существование пользователя по его электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <returns>
        /// <c>true</c>, если пользователь с указанной электронной почтой существует, иначе <c>false</c>.
        /// </returns>
        Task<bool> UserExistsByEmailAsync(string email);

        /// <summary>
        /// Добавляет нового пользователя в базу данных.
        /// </summary>
        /// <param name="user">Пользователь для добавления.</param>
        Task AddUserAsync(User user);

        /// <summary>
        /// Обновляет данные пользователя в базе данных.
        /// </summary>
        /// <param name="user">Пользователь для обновления.</param>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Удаляет пользователя из базы данных.
        /// </summary>
        /// <param name="user">Пользователь для удаления.</param>
        Task DeleteUserAsync(User user);

        /// <summary>
        /// Назначает роль пользователю.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="roleId">Идентификатор роли.</param>
        Task AssignRoleAsync(int userId, int roleId);

        /// <summary>
        /// Удаляет роль у пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="roleId">Идентификатор роли.</param>
        Task RemoveRoleAsync(int userId, int roleId);
        #endregion
    }

    #endregion
}