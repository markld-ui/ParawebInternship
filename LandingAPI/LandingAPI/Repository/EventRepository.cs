﻿#region Заголовок файла

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
using Microsoft.AspNetCore.Mvc;

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
        public async Task<(ICollection<Event> Events, int TotalCount)> GetEventsAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortField = "EventId",
            bool ascending = true)
        {
            var query = _context.Events
                .Include(e => e.CreatedBy)
                .Include(e => e.File)
                .AsQueryable();

            query = sortField switch
            {
                "Title" => ascending ? query.OrderBy(e => e.Title) : query.OrderByDescending(e => e.Title),
                "StartDate" => ascending ? query.OrderBy(e => e.StartDate) : query.OrderByDescending(e => e.StartDate),
                "EndDate" => ascending ? query.OrderBy(e => e.EndDate) : query.OrderByDescending(e => e.EndDate),
                _ => ascending ? query.OrderBy(e => e.EventId) : query.OrderByDescending(e => e.EventId)
            };

            var totalCount = await query.CountAsync();
            var events = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (events, totalCount);
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

        /// <summary>
        /// Добавляет новое событие в базу данных.
        /// </summary>
        /// <param name="event_">Событие для добавления.</param>
        public async Task AddEventAsync(Event event_)
        {
            await _context.Events.AddAsync(event_);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет существующее событие в базе данных.
        /// </summary>
        /// <param name="event_">Событие для обновления.</param>
        public async Task UpdateEventAsync(Event event_)
        {
            _context.Events.Update(event_);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет событие из базы данных.
        /// </summary>
        /// <param name="event_">Событие для удаления.</param>
        public async Task DeleteEventAsync(Event event_)
        {
            _context.Events.Remove(event_);
            await _context.SaveChangesAsync();
        }
        #endregion
    }

    #endregion
}