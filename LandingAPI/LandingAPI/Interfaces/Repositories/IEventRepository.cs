#region Заголовок файла

/// <summary>
/// Файл: IEventRepository.cs
/// Интерфейс репозитория для работы с сущностью <see cref="Event"/>.
/// Определяет методы для работы с событиями в базе данных.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace LandingAPI.Interfaces.Repositories
{
    #region Интерфейс IEventRepository

    /// <summary>
    /// Интерфейс репозитория для работы с сущностью <see cref="Event"/>.
    /// </summary>
    public interface IEventRepository
    {
        #region Методы

        /// <summary>
        /// Получает список всех событий.
        /// </summary>
        /// <returns>Коллекция событий.</returns>
        Task<ICollection<Event>> GetEventsAsync();

        /// <summary>
        /// Получает событие по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <returns>Событие, если найдено, иначе <c>null</c>.</returns>
        Task<Event> GetEventByIdAsync(int id);

        /// <summary>
        /// Получает список событий, связанных с определенным пользователем.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Коллекция событий, связанных с пользователем.</returns>
        Task<ICollection<Event>> GetEventsByUserIdAsync(int userId);

        /// <summary>
        /// Проверяет существование события по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <returns>
        /// <c>true</c>, если событие с указанным идентификатором существует, иначе <c>false</c>.
        /// </returns>
        Task<bool> EventExistsByIdAsync(int id);

        #endregion
    }

    #endregion
}