#region Заголовок файла

/// <summary>
/// Файл: EventsController.cs
/// Контроллер для управления событиями (Events).
/// Предоставляет методы для получения списка событий, поиска события по идентификатору и фильтрации событий по идентификатору пользователя.
/// </summary>

#endregion

#region Пространства имен

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LandingAPI.DTO;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace LandingAPI.Controllers
{
    #region Класс EventsController

    /// <summary>
    /// Контроллер для управления событиями.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        #region Поля и свойства

        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EventsController"/>.
        /// </summary>
        /// <param name="eventRepository">Репозиторий для работы с событиями.</param>
        /// <param name="mapper">Объект для маппинга данных между моделями и DTO.</param>
        public EventsController(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        #endregion

        #region Методы

        #region GetEvents

        /// <summary>
        /// Получает список всех событий.
        /// </summary>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если события не найдены.
        /// - 200 OK с списком событий в формате <see cref="EventDTO"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), 200)]
        public async Task<IActionResult> GetEvents()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var events = await _eventRepository.GetEventsAsync();
            if (events == null)
                return NotFound();

            var eventDtos = _mapper.Map<List<EventDTO>>(events);
            return Ok(eventDtos);
        }

        #endregion

        #region GetEvent

        /// <summary>
        /// Получает событие по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если событие с указанным идентификатором не найдено.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с данными события в формате <see cref="EventDTO"/>.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventDTO), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEvent(int id)
        {
            if (!await _eventRepository.EventExistsByIdAsync(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var event_ = await _eventRepository.GetEventByIdAsync(id);
            var eventDto = _mapper.Map<EventDTO>(event_);
            return Ok(eventDto);
        }

        #endregion

        #region GetEventsByUserId

        /// <summary>
        /// Получает список событий, связанных с определенным пользователем.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если события не найдены.
        /// - 200 OK с списком событий в формате <see cref="EventDTO"/>.
        /// </returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEventsByUserId(int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var events = await _eventRepository.GetEventsByUserIdAsync(userId);
            if (events == null)
                return NotFound();

            var eventDtos = _mapper.Map<List<EventDTO>>(events);
            return Ok(eventDtos);
        }

        #endregion

        #endregion
    }

    #endregion
}