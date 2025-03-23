#region Заголовок файла

/// <summary>
/// Файл: EventRepository.cs
/// Реализация репозитория для работы с сущностью <see cref="Event"/>.
/// Предоставляет методы для доступа к данным о событиях в базе данных.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using LandingAPI.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace LandingAPI.Repository
{
    #region Класс EventRepository

    /// <summary>
    /// Реализация репозитория для работы с сущностью <see cref="Event"/>.
    /// </summary>
    public class EventRepository : IEventRepository
    {
        #region Поля

        private readonly DataContext _context;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EventRepository"/>.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public EventRepository(DataContext context)
        {
            _context = context;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Проверяет существование события по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <returns>
        /// <c>true</c>, если событие с указанным идентификатором существует, иначе <c>false</c>.
        /// </returns>
        public async Task<bool> EventExistsByIdAsync(int id)
        {
            return await _context.Events.AnyAsync(e => e.EventId == id);
        }

        /// <summary>
        /// Получает событие по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <returns>Событие, если найдено, иначе <c>null</c>.</returns>
        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);
        }

        /// <summary>
        /// Получает список всех событий, отсортированных по идентификатору.
        /// </summary>
        /// <returns>Коллекция событий.</returns>
        public async Task<ICollection<Event>> GetEventsAsync()
        {
            return await _context.Events.OrderBy(e => e.EventId).ToListAsync();
        }

        /// <summary>
        /// Получает список событий, созданных определенным пользователем.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Коллекция событий, созданных пользователем.</returns>
        public async Task<ICollection<Event>> GetEventsByUserIdAsync(int userId)
        {
            return await _context.Events.Where(e => e.CreatedById == userId).ToListAsync();
        }

        public async Task AddEventAsync(Event event_)
{
            await _context.Events.AddAsync(event_);
        await _context.SaveChangesAsync();
}

public async Task UpdateEventAsync(Event event_)
        {
            _context.Events.Update(event_);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(Event event_)
        {
            _context.Events.Remove(event_);
            await _context.SaveChangesAsync();
        }
        #endregion
    }

    #endregion
}