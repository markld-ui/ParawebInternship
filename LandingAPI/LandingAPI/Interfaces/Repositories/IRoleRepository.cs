#region Заголовок файла

/// <summary>
/// Файл: IRoleRepository.cs
/// Интерфейс репозитория для работы с сущностью <see cref="Role"/>.
/// Определяет методы для работы с ролями в базе данных.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace LandingAPI.Interfaces.Repositories
{
    #region Интерфейс IRoleRepository

    /// <summary>
    /// Интерфейс репозитория для работы с сущностью <see cref="Role"/>.
    /// </summary>
    public interface IRoleRepository
    {
        #region Методы

        /// <summary>
        /// Получает список всех ролей.
        /// </summary>
        /// <returns>Коллекция ролей.</returns>
        Task<ICollection<Role>> GetRolesAsync();

        /// <summary>
        /// Получает роль по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <returns>Роль, если найдена, иначе <c>null</c>.</returns>
        Task<Role> GetRoleByIdAsync(int id);

        /// <summary>
        /// Получает роль по ее названию.
        /// </summary>
        /// <param name="name">Название роли.</param>
        /// <returns>Роль, если найдена, иначе <c>null</c>.</returns>
        Task<Role> GetRoleByNameAsync(string name);

        /// <summary>
        /// Добавляет новую роль в базу данных.
        /// </summary>
        /// <param name="role">Роль для добавления.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task AddRoleAsync(Role role);

        /// <summary>
        /// Обновляет существующую роль в базе данных.
        /// </summary>
        /// <param name="role">Роль для обновления.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task UpdateRoleAsync(Role role);

        /// <summary>
        /// Удаляет роль из базы данных.
        /// </summary>
        /// <param name="role">Роль для удаления.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task DeleteRoleAsync(Role role);

        #endregion
    }

    #endregion
}