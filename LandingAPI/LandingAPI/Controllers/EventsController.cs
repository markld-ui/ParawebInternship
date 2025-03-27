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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace LandingAPI.Controllers
{
    #region Класс EventsController

    /// <summary>
    /// Контроллер для управления событиями.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("99.0")]
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
        /// <param name="filesRepository">Репозиторий для работы с файлами.</param>
        /// <param name="fileService">Сервис для работы с файлами</param>
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
        /// Получает список событий с пагинацией и сортировкой
        /// </summary>
        /// <remarks>
        /// ### Параметры запроса:
        /// - **page** (необязательный) - номер страницы (по умолчанию 1)
        /// - **size** (необязательный) - количество элементов на странице (по умолчанию 10)
        /// - **sort** (необязательный) - поле для сортировки (доступные значения: "EventId", "Title", "StartDate", "EndDate") (по умолчанию "StartDate")
        /// - **asc** (необязательный) - направление сортировки (true - по возрастанию, false - по убыванию) (по умолчанию true)
        /// 
        /// ### Пример запроса:
        /// GET /api/Events?page=2&size=5&sort=Title&asc=false
        /// 
        /// ### Возможные коды ответа:
        /// - 200 - Успешный запрос, возвращает список событий
        /// - 400 - Неверные параметры запроса
        /// - 404 - События не найдены
        /// </remarks>
        /// <response code="200">Возвращает пагинированный список событий</response>
        /// <response code="400">Если параметры запроса невалидны</response>
        /// <response code="404">Если события не найдены</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("99.0")]
        [ProducesResponseType(typeof(PagedResponse<EventShortDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
        /// Получает детальную информацию о событии по его идентификатору
        /// </summary>
        /// <remarks>
        /// ### Параметры:
        /// - **id** (обязательный) - уникальный идентификатор события
        /// 
        /// ### Пример запроса:
        /// GET /api/Events/5
        /// 
        /// ### Возможные коды ответа:
        /// - 200 - Успешный запрос, возвращает детали события
        /// - 400 - Неверный формат идентификатора
        /// - 404 - Событие с указанным ID не найдено
        /// 
        /// ### Пример ответа:
        /// ```json
        /// {
        ///   "eventId": 5,
        ///   "title": "Техническая конференция",
        ///   "description": "Ежегодная конференция для разработчиков",
        ///   "startDate": "2023-12-01T09:00:00",
        ///   "endDate": "2023-12-03T18:00:00",
        ///   "location": "Москва, Крокус Сити Холл",
        ///   "createdAt": "2023-11-15T10:00:00",
        ///   "createdBy": {
        ///     "userId": 1,
        ///     "userName": "admin"
        ///   },
        ///   "file": {
        ///     "fileId": 12,
        ///     "fileName": "agenda.pdf",
        ///     "downloadUrl": "https://api.example.com/api/Files/download/12"
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <param name="id">Идентификатор события (целое число больше 0)</param>
        /// <response code="200">Возвращает детальную информацию о событии</response>
        /// <response code="400">Если идентификатор невалиден</response>
        /// <response code="404">Если событие не найдено</response>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("99.0")]
        [ProducesResponseType(typeof(EventDetailsDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
                    DownloadUrl = _fileService.GetFileUrl(event_.File, Request)
                } : null
            });
        }

        #endregion

        #region GetEventsByUserIdAsync

        /// <summary>
        /// Получает список событий, связанных с определенным пользователем.
        /// </summary>
        /// <remarks>
        /// ### Параметры:
        /// - **userId** (обязательный) - уникальный идентификатор пользователя
        /// 
        /// ### Пример запроса:
        /// GET /api/Events/search?userId=1
        /// 
        /// ### Возможные коды ответа:
        /// - 200 - Успешный запрос, возвращает список событий
        /// - 400 - Неверный формат данных или отсутствует обязательный параметр
        /// - 404 - События для указанного пользователя не найдены
        /// 
        /// ### Пример ответа:
        /// ```json
        /// [
        ///   {
        ///     "eventId": 5,
        ///     "title": "Техническая конференция",
        ///     "description": "Ежегодная конференция для разработчиков",
        ///     "startDate": "2023-12-01T09:00:00",
        ///     "endDate": "2023-12-03T18:00:00",
        ///     "location": "Москва, Крокус Сити Холл"
        ///   },
        ///   {
        ///     "eventId": 6,
        ///     "title": "Вебинар по искусственному интеллекту",
        ///     "description": "Обсуждение последних трендов в AI",
        ///     "startDate": "2023-11-20T15:00:00",
        ///     "endDate": "2023-11-20T16:30:00",
        ///     "location": "Онлайн"
        ///   }
        /// ]
        /// ```
        /// </remarks>
        /// <param name="userId">Идентификатор пользователя (целое число больше 0)</param>
        /// <response code="200">Возвращает список событий, связанных с пользователем</response>
        /// <response code="400">Если модель данных невалидна или отсутствует параметр userId</response>
        /// <response code="404">Если события не найдены для указанного пользователя</response>
        [HttpGet("search")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("99.0")]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
        /// - 200 OK с данными созданного события в формате <see cref="EventDetailsDTO"/>.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// 
        /// ### Пример запроса:
        /// POST /api/Events
        /// ```json
        /// {
        ///   "title": "Новая конференция",
        ///   "description": "Описание новой конференции",
        ///   "startDate": "2023-12-10T09:00:00",
        ///   "endDate": "2023-12-12T18:00:00",
        ///   "location": "Санкт-Петербург, Экспофорум",
        ///   "fileId": 10
        /// }
        /// ```
        /// 
        /// ### Пример ответа:
        /// ```json
        /// {
        ///   "eventId": 7,
        ///   "title": "Новая конференция",
        ///   "description": "Описание новой конференции",
        ///   "startDate": "2023-12-10T09:00:00",
        ///   "endDate": "2023-12-12T18:00:00",
        ///   "location": "Санкт-Петербург, Экспофорум",
        ///   "createdAt": "2023-11-15T10:00:00",
        ///   "createdBy": {
        ///     "userId": 1,
        ///     "userName": "admin"
        ///   },
        ///   "file": {
        ///     "fileId": 10,
        ///     "fileName": "agenda.pdf",
        ///     "downloadUrl": "https://api.example.com/api/Files/download/10"
        ///   }
        /// }
        /// ```
        /// </remarks>
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [MapToApiVersion("99.0")]
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
        /// - 200 OK с данными обновленного события в формате <see cref="EventDetailsDTO"/>.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если событие с указанным идентификатором не найдено.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// 
        /// ### Пример запроса:
        /// PUT /api/Events/1
        /// ```json
        /// {
        ///   "title": "Обновленная конференция",
        ///   "description": "Обновленное описание конференции",
        ///   "startDate": "2023-12-11T09:00:00",
        ///   "endDate": "2023-12-13T18:00:00",
        ///   "location": "Санкт-Петербург, Невский проспект",
        ///   "fileId": 12,
        ///   "removeFile": false
        /// }
        /// ```
        /// 
        /// ### Пример ответа:
        /// ```json
        /// {
        ///   "eventId": 1,
        ///   "title": "Обновленная конференция",
        ///   "description": "Обновленное описание конференции",
        ///   "startDate": "2023-12-11T09:00:00",
        ///   "endDate": "2023-12-13T18:00:00",
        ///   "location": "Санкт-Петербург, Невский проспект",
        ///   "createdAt": "2023-11-15T10:00:00",
        ///   "createdBy": {
        ///     "userId": 1,
        ///     "userName": "admin"
        ///   },
        ///   "file": {
        ///     "fileId": 12,
        ///     "fileName": "updated_agenda.pdf",
        ///     "downloadUrl": "https://api.example.com/api/Files/download/12"
        ///   }
        /// }
        /// ```
        /// </remarks>
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [MapToApiVersion("99.0")]
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
        /// - 204 NoContent, если событие успешно удалено.
        /// - 404 NotFound, если событие с указанным идентификатором не найдено.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// 
        /// ### Пример запроса:
        /// DELETE /api/Events/1
        /// 
        /// ### Пример ответа:
        /// - 204 NoContent (если событие успешно удалено)
        /// 
        /// - 404 NotFound (если событие не найдено)
        /// </remarks>
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [MapToApiVersion("99.0")]
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
        /// Преобразует объект <see cref="Event"/> в объект <see cref="EventDetailsDTO"/>.
        /// </summary>
        /// <param name="event_">Объект мероприятия, который нужно преобразовать.</param>
        /// <returns>
        /// Возвращает объект <see cref="EventDetailsDTO"/>, содержащий детали мероприятия.
        /// </returns>
        /// <remarks>
        /// Метод создает DTO (Data Transfer Object) для передачи данных о мероприятии.
        /// 
        /// ### Пример возвращаемого объекта:
        /// ```json
        /// {
        ///   "eventId": 1,
        ///   "title": "Конференция 2023",
        ///   "description": "Описание конференции 2023 года.",
        ///   "startDate": "2023-12-01T10:00:00",
        ///   "endDate": "2023-12-01T17:00:00",
        ///   "location": "Москва, Кремль",
        ///   "createdAt": "2023-01-01T12:00:00",
        ///   "createdBy": {
        ///     "userId": 1,
        ///     "userName": "admin"
        ///   },
        ///   "file": {
        ///     "fileId": 10,
        ///     "fileName": "agenda.pdf",
        ///     "downloadUrl": "https://api.example.com/api/Files/download/10"
        ///   }
        /// }
        /// ```
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
                    DownloadUrl = _fileService.GetFileUrl(event_.File, Request)
                } : null
            };
        }
        #endregion

        #endregion
    }

    #endregion
}