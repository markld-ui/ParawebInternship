#region Заголовок файла

/// <summary>
/// Файл: EventsController.cs
/// Контроллер для управления событиями (Events).
/// Предоставляет методы для получения списка событий, поиска события по идентификатору и фильтрации событий по идентификатору пользователя.
/// </summary>

#endregion

#region Пространства имен

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LandingAPI.DTO.Common;
using LandingAPI.DTO.Events;
using LandingAPI.DTO.News;
using LandingAPI.Helper;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Models;
using LandingAPI.Repository;
using LandingAPI.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IFilesRepository _filesRepository;
        private readonly FileService _fileService;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EventsController"/>.
        /// </summary>
        /// <param name="eventRepository">Репозиторий для работы с событиями.</param>
        /// <param name="mapper">Объект для маппинга данных между моделями и DTO.</param>
        public EventsController(IEventRepository eventRepository, IMapper mapper, IFilesRepository filesRepository, FileService fileService)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _filesRepository = filesRepository;
            _fileService = fileService;
        }

        #endregion

        #region Методы

        #region GetEventsAsync

        /// <summary>
        /// Получает список всех событий.
        /// </summary>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если события не найдены.
        /// - 200 OK с списком событий в формате <see cref="EventShortDTO"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<EventShortDTO>), 200)]
        public async Task<IActionResult> GetEventsAsync(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string sort = "StartDate",
            [FromQuery] bool asc = true)
        {
            var (events, totalCount) = await _eventRepository.GetEventsAsync(page, size, sort, asc);
            if (events == null || !events.Any())
                return NotFound("События не найдены");

            var dtos = events.Select(e => new EventShortDTO
            {
                EventId = e.EventId,
                Title = e.Title,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Location = e.Location,
                AuthorName = e.CreatedBy.Username,
                HasAttachment = e.FileId.HasValue
            }).ToList();

            return Ok(new PagedResponse<EventShortDTO>
            {
                Data = dtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = size
            });
        }

        #endregion

        #region GetEventByIdAsync

        /// <summary>
        /// Получает событие по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если событие с указанным идентификатором не найдено.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с данными события в формате <see cref="EventDetailsDTO"/>.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventDetailsDTO), 200)]
        public async Task<IActionResult> GetEvent(int id)
        {
            var event_ = await _eventRepository.GetEventByIdAsync(id);
            if (event_ == null)
                return NotFound("Событие не найдено");

            return Ok(new EventDetailsDTO
            {
                EventId = event_.EventId,
                Title = event_.Title,
                Description = event_.Description,
                StartDate = event_.StartDate,
                EndDate = event_.EndDate,
                Location = event_.Location,
                CreatedAt = event_.CreatedAt,
                CreatedBy = new AuthorDTO
                {
                    UserId = event_.CreatedById,
                    UserName = event_.CreatedBy.Username
                },
                File = event_.FileId.HasValue ? new FileInfoDTO
                {
                    FileId = event_.FileId.Value,
                    FileName = event_.File.FileName,
                    DownloadUrl = Url.Action("DownloadFile", "Files", new { id = event_.FileId }, Request.Scheme)
                } : null
            });
        }

        #endregion

        #region GetEventsByUserIdAsync

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
        public async Task<IActionResult> GetEventsByUserIdAsync(int userId)
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

        #region CreateEventAsync

        /// <summary>
        /// Создает новое событие.
        /// </summary>
        /// <param name="model">Модель данных для создания события, содержащая название, описание, даты начала и окончания, местоположение и идентификатор файла.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с данными созданного события.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<EventDetailsDTO>> CreateEventAsync([FromForm] CreateEventDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int? fileId = null;

            // Обработка загружаемого файла
            if (dto.File != null && dto.File.Length > 0)
            {
                var uploadedFile = await _fileService.UploadFileAsync(dto.File);
                fileId = uploadedFile.FileId;
            }
            else if (dto.FileId.HasValue)
            {
                fileId = dto.FileId.Value;
            }

            var event_ = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Location = dto.Location,
                CreatedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CreatedAt = DateTime.UtcNow,
                FileId = fileId
            };

            await _eventRepository.AddEventAsync(event_);
            return Ok(await MapToDetailsDTO(event_));
        }

        #endregion

        #region UpdateEventAsync

        /// <summary>
        /// Обновляет существующее событие.
        /// </summary>
        /// <param name="id">Идентификатор события, которое нужно обновить.</param>
        /// <param name="model">Модель данных для обновления события, содержащая новое название, описание, даты начала и окончания, местоположение и идентификатор файла.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если событие с указанным идентификатором не найдено.
        /// - 200 OK с данными обновленного события.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<EventDetailsDTO>> UpdateEventAsync(int id, [FromBody] UpdateEventDTO dto)
        {
            var event_ = await _eventRepository.GetEventByIdAsync(id);
            if (event_ == null)
                return NotFound("Событие не найдено");

            if (!string.IsNullOrEmpty(dto.Title))
                event_.Title = dto.Title;

            if (!string.IsNullOrEmpty(dto.Description))
                event_.Description = dto.Description;

            if (dto.StartDate.HasValue)
                event_.StartDate = dto.StartDate.Value;

            if (dto.EndDate.HasValue)
                event_.EndDate = dto.EndDate.Value;

            if (dto.Location != null)
                event_.Location = dto.Location;

            if (dto.RemoveFile == true && event_.FileId.HasValue)
            {
                event_.FileId = null;
            }
            else if (dto.FileId.HasValue)
            {
                event_.FileId = dto.FileId.Value;
            }

            await _eventRepository.UpdateEventAsync(event_);
            return Ok(await MapToDetailsDTO(event_));
        }

        #endregion

        #region DeleteEventAsync

        /// <summary>
        /// Удаляет событие по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор события, которое нужно удалить.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если событие с указанным идентификатором не найдено.
        /// - 204 NoContent, если событие успешно удалено.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventAsync(int id)
        {
            var event_ = await _eventRepository.GetEventByIdAsync(id);
            if (event_ == null)
                return NotFound();

            await _eventRepository.DeleteEventAsync(event_);
            return NoContent();
        }

        #endregion

        #region MapToDetailsDTO

        /// <summary>
        /// Преобразует объект <see cref="Event "/> в объект <see cref="EventDetailsDTO"/>.
        /// </summary>
        /// <param name="event_">Объект мероприятия, который нужно преобразовать.</param>
        /// <returns>
        /// Возвращает объект <see cref="EventDetailsDTO"/>, содержащий детали мероприятия.
        /// </returns>
        /// <remarks>
        /// Метод создает DTO (Data Transfer Object) для передачи данных о мероприятии.
        /// </remarks>
        private async Task<EventDetailsDTO> MapToDetailsDTO(Event event_)
        {
            return new EventDetailsDTO
            {
                EventId = event_.EventId,
                Title = event_.Title,
                Description = event_.Description,
                StartDate = event_.StartDate,
                EndDate = event_.EndDate,
                Location = event_.Location,
                CreatedAt = event_.CreatedAt,
                CreatedBy = new AuthorDTO
                {
                    UserId = event_.CreatedById,
                    UserName = event_.CreatedBy.Username
                },
                File = event_.FileId.HasValue ? new FileInfoDTO
                {
                    FileId = event_.FileId.Value,
                    FileName = event_.File.FileName,
                    DownloadUrl = Url.Action("DownloadFile", "Files", new { id = event_.FileId }, Request.Scheme)
                } : null
            };
        }
        #endregion

        #endregion
    }

    #endregion
}