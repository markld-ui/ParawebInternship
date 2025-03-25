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
        /// <param name="pageNumber">Номер страницы для получения (по умолчанию 1).</param>
        /// <param name="pageSize">Количество мероприятий на странице (по умолчанию 10).</param>
        /// <param name="sortField">Поле, по которому будет выполнена сортировка (по умолчанию "Event Id").</param>
        /// <param name="ascending">Указывает, должна ли сортировка быть по возрастанию (по умолчанию true).</param>
        /// <returns>Кортеж, содержащий коллекцию мероприятий и общее количество мероприятий.</returns>
        Task<(ICollection<Event> Events, int TotalCount)> GetEventsAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortField = "EventId",
            bool ascending = true);

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

        /// <summary>
        /// Добавляет новое событие в базу данных.
        /// </summary>
        /// <param name="event_">Событие для добавления.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task AddEventAsync(Event event_);

        /// <summary>
        /// Обновляет существующее событие в базе данных.
        /// </summary>
        /// <param name="event_">Событие для обновления.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task UpdateEventAsync(Event event_);

        /// <summary>
        /// Удаляет событие из базы данных.
        /// </summary>
        /// <param name="event_">Событие для удаления.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task DeleteEventAsync(Event event_);

        #endregion
    }

    #endregion
}