#region Заголовок файла

/// <summary>
/// Файл: NewsController.cs
/// Контроллер для управления новостями.
/// Предоставляет методы для получения списка новостей, поиска новости по идентификатору и фильтрации новостей по заголовку.
/// Доступ к методам контроллера разрешен только пользователям с ролью "Admin".
/// </summary>

#endregion

#region Пространства имен

using AutoMapper;
using LandingAPI.DTO.Common;
using LandingAPI.DTO.News;
using LandingAPI.Helper;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Models;
using LandingAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

#endregion

namespace LandingAPI.Controllers
{
    #region Класс NewsController

    /// <summary>
    /// Контроллер для управления новостями.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("99.0")]
    [ApiController]
    public class NewsController : Controller
    {
        #region Поля и свойства

        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        private readonly IFilesRepository _filesRepository;
        private readonly IUserRepository _userRepository;
        private readonly FileService _fileService;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NewsController"/>.
        /// </summary>
        /// <param name="newsRepository">Репозиторий для работы с новостями.</param>
        /// <param name="mapper">Объект для маппинга данных между моделями и DTO.</param>
        /// <param name="filesRepository">Репозиторий для работы с файлами.</param>
        /// <param name="fileService">Сервис для работы с файлами</param>
        public NewsController(INewsRepository newsRepository, 
            IMapper mapper, 
            IFilesRepository filesRepository, 
            IUserRepository userRepository,
            FileService fileService)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _filesRepository = filesRepository;
            _fileService = fileService;
            _userRepository = userRepository;
        }

        #endregion

        #region Методы

        #region GetNewsAsync

        /// <summary>
        /// Получает список всех новостей.
        /// </summary>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с списком новостей в формате <see cref="NewsShortDTO"/>.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если новости не найдены.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/news?page=1&size=10&sort=NewsId&asc=true
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// {
        ///   "data": [
        ///     {
        ///       "newsId": 1,
        ///       "title": "Заголовок новости 1",
        ///       "createdAt": "2023-01-01T12:00:00Z",
        ///       "authorName": "Автор 1",
        ///       "hasAttachment": false
        ///     },
        ///     {
        ///       "newsId": 2,
        ///       "title": "Заголовок новости 2",
        ///       "createdAt": "2023-01-02T12:00:00Z",
        ///       "authorName": "Автор 2",
        ///       "hasAttachment": true
        ///     }
        ///   ],
        ///   "totalCount": 2,
        ///   "page": 1,
        ///   "pageSize": 10
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (новости не найдены):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Новости не найдены"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// - **HTTP/1.1 400 Bad Request**
        /// ```json
        /// {
        ///   "errors": {
        ///     "page": ["Page must be greater than 0."],
        ///     "size": ["Size must be greater than 0."]
        ///   }
        /// }
        /// ```
        /// </remarks>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("99.0")]
        [ProducesResponseType(typeof(PagedResponse<NewsShortDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetNewsAsync(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string sort = "NewsId",
            [FromQuery] bool asc = true)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (news, totalCount) = await _newsRepository.GetNewsAsync(page, size, sort, asc);
            if (news == null || !news.Any())
                return NotFound("Новости не найдены");

            var newsDtos = news.Select(n => new NewsShortDTO
            {
                NewsId = n.NewsId,
                Title = n.Title,
                CreatedAt = n.CreatedAt,
                AuthorName = n.CreatedBy.Username,
                HasAttachment = n.FileId.HasValue
            }).ToList();

            return Ok(new PagedResponse<NewsShortDTO>
            {
                Data = newsDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = size
            });
        }

        #endregion

        #region GetNewAsync

        /// <summary>
        /// Получает новость по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор новости.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с данными новости в формате <see cref="NewsDetailsDTO"/>.
        /// - 404 NotFound, если новость с указанным идентификатором не найдена.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/news/{id}
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// {
        ///   "newsId": 1,
        ///   "title": "Заголовок новости",
        ///   "content": "Содержимое новости",
        ///   "createdAt": "2023-01-01T12:00:00Z",
        ///   "authorName": "Автор",
        ///   "hasAttachment": false
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (новость не найдена):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Новость не найдена"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// - **HTTP/1.1 400 Bad Request**
        /// ```json
        /// {
        ///   "error": "Некорректный идентификатор новости"
        /// }
        /// ```
        /// </remarks>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("99.0")]
        [ProducesResponseType(typeof(NewsDetailsDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNewAsync(int id)
        {
            if (!await _newsRepository.NewsExistsAsync(id))
                return NotFound("Новость не найдена");

            var news = await _newsRepository.GetNewsAsync(id);
            return Ok(await MapToDetailsDTO(news));
        }

        #endregion

        #region GetNewByTitleAsync

        /// <summary>
        /// Получает новость по заголовку.
        /// </summary>
        /// <param name="title">Заголовок новости.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с данными новости в формате <see cref="NewsDetailsDTO"/>.
        /// - 404 NotFound, если новость с указанным заголовком не найдена.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/news/search/{title}
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// {
        ///   "newsId": 1,
        ///   "title": "Заголовок новости",
        ///   "content": "Содержимое новости",
        ///   "createdAt": "2023-01-01T12:00:00Z",
        ///   "authorName": "Автор",
        ///   "hasAttachment": false
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (новость не найдена):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Новость с указанным заголовком не найдена"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// - **HTTP/1.1 400 Bad Request**
        /// ```json
        /// {
        ///   "error": "Некорректный заголовок новости"
        /// }
        /// ```
        /// </remarks>
        [HttpGet("search/{title}")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("99.0")]
        [ProducesResponseType(typeof(IEnumerable<NewsDetailsDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNewByTitleAsync(string title)
        {
            if (!await _newsRepository.NewsExistsByTitleAsync(title))
                return NotFound();

            var news = await _newsRepository.GetNewsByTitleAsync(title);
            var newsDtos = _mapper.Map<NewsDetailsDTO>(news);
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(newsDtos);
        }

        #endregion

        #region CreateNews

        /// <summary>
        /// Создает новую новость.
        /// </summary>
        /// <param name="model">Модель данных для создания новости, содержащая заголовок, содержание и идентификатор файла.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с данными созданной новости.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// 
        /// ### Пример запроса:
        /// POST /api/news
        /// 
        /// **Тело запроса:**
        /// ```json
        /// {
        ///   "title": "Заголовок новости",
        ///   "content": "Содержимое новости",
        ///   "file": "файл_с_изображением.jpg" // (файл загружается через форму)
        /// }
        /// ```
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// {
        ///   "newsId": 1,
        ///   "title": "Заголовок новости",
        ///   "content": "Содержимое новости",
        ///   "createdAt": "2023-01-01T12:00:00Z",
        ///   "authorName": "Автор",
        ///   "hasAttachment": true
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// - **HTTP/1.1 400 Bad Request**
        /// ```json
        /// {
        ///   "error": "Некорректные данные для создания новости"
        /// }
        /// ```
        /// </remarks>
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [MapToApiVersion("99.0")]
        [HttpPost]
        public async Task<ActionResult<NewsDetailsDTO>> CreateNews([FromForm] CreateNewsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int? fileId = null;
            if (dto.File != null && dto.File.Length > 0)
            {
                var uploadedFile = await _fileService.UploadFileAsync(dto.File);
                fileId = uploadedFile.FileId;
            }

            var news = new News
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CreatedAt = DateTime.UtcNow,
                FileId = fileId
            };

            await _newsRepository.AddNewsAsync(news);

            return Ok(await MapToDetailsDTO(news));
        }

        #endregion

        #region UpdateNews

        /// <summary>
        /// Обновляет существующую новость.
        /// </summary>
        /// <param name="id">Идентификатор новости, которую нужно обновить.</param>
        /// <param name="model">Модель данных для обновления новости, содержащая новый заголовок, содержание и идентификатор файла.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с данными обновленной новости.
        /// - 404 NotFound, если новость с указанным идентификатором не найдена.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// 
        /// ### Пример запроса:
        /// PUT /api/news/{id}
        /// 
        /// **Тело запроса:**
        /// ```json
        /// {
        ///   "title": "Новый заголовок новости",
        ///   "content": "Новое содержание новости",
        ///   "removeFile": true, // (если нужно удалить существующий файл)
        ///   "newFile": "новый_файл_с_изображением.jpg" // (файл загружается через форму)
        /// }
        /// ```
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// ```json
        /// {
        ///   "newsId": 1,
        ///   "title": "Новый заголовок новости",
        ///   "content": "Новое содержание новости",
        ///   "createdAt": "2023-01-01T12:00:00Z",
        ///   "authorName": "Автор",
        ///   "hasAttachment": true
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (новость не найдена):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Новость с указанным идентификатором не найдена"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// - **HTTP/1.1 400 Bad Request**
        /// ```json
        /// {
        ///   "error": "Некорректные данные для обновления новости"
        /// }
        /// ```
        /// </remarks>
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [MapToApiVersion("99.0")]
        [HttpPut("{id}")]
        public async Task<ActionResult<NewsDetailsDTO>> UpdateNews(int id, [FromForm] UpdateNewsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var news = await _newsRepository.GetNewsAsync(id);
            if (news == null)
                return NotFound();

            // Обработка файла
            if (dto.RemoveFile == true && news.FileId.HasValue)
            {
                await _fileService.DeleteFileAsync(news.FileId.Value);
                news.FileId = null;
            }
            else if (dto.NewFile != null && dto.NewFile.Length > 0)
            {
                if (news.FileId.HasValue)
                    await _fileService.DeleteFileAsync(news.FileId.Value);

                var uploadedFile = await _fileService.UploadFileAsync(dto.NewFile);
                news.FileId = uploadedFile.FileId;
            }

            news.Title = dto.Title;
            news.Content = dto.Content;

            await _newsRepository.UpdateNewsAsync(news);
            return Ok(await MapToDetailsDTO(news));
        }

        #endregion

        #region DeleteNews

        /// <summary>
        /// Удаляет новость по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор новости, которую нужно удалить.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 204 NoContent, если новость успешно удалена.
        /// - 404 NotFound, если новость с указанным идентификатором не найдена.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// 
        /// ### Пример запроса:
        /// DELETE /api/news/{id}
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 204 No Content**
        /// 
        /// ### Пример ошибки (новость не найдена):
        /// - **HTTP/1.1 404 Not Found**
        /// ```json
        /// {
        ///   "error": "Новость не найдена."
        /// }
        /// ```
        /// </remarks>
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [MapToApiVersion("99.0")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _newsRepository.GetNewsAsync(id);
            if (news == null)
                return NotFound("Новость не найдена.");

            await _newsRepository.DeleteNewsAsync(news);
            return NoContent();
        }

        #endregion

        #region MapToDetailsDTO

        /// <summary>
        /// Преобразует объект <see cref="News"/> в объект <see cref="NewsDetailsDTO"/>.
        /// </summary>
        /// <param name="news">Объект новости, который нужно преобразовать.</param>
        /// <returns>
        /// Возвращает объект <see cref="NewsDetailsDTO"/>, содержащий детали новости.
        /// </returns>
        /// <remarks>
        /// Метод создает DTO (Data Transfer Object) для передачи данных о новости,
        /// включая информацию о создателе и файле, если он доступен.
        /// 
        /// ### Пример возвращаемого объекта:
        /// ```json
        /// {
        ///   "newsId": 1,
        ///   "title": "Заголовок новости",
        ///   "content": "Содержание новости",
        ///   "createdAt": "2023-01-01T12:00:00Z",
        ///   "createdBy": {
        ///     "userId": 42,
        ///     "userName": "Автор"
        ///   },
        ///   "file": {
        ///     "fileId": 101,
        ///     "fileName": "файл.pdf",
        ///     "downloadUrl": "https://example.com/files/download/101"
        ///   }
        /// }
        /// ```
        /// 
        /// Если файл отсутствует, поле `file` будет равно `null`.
        /// </remarks>
        private async Task<NewsDetailsDTO> MapToDetailsDTO(News news)
        {
            string userName = string.Empty;
            if (news.CreatedBy != null)
            {
                userName = news.CreatedBy.Username;
            }
            else
            {
                var user = await _userRepository.GetUserByIdAsync(news.CreatedById);
                userName = user?.Username ?? string.Empty;
            }

            return new NewsDetailsDTO
            {
                NewsId = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                CreatedAt = news.CreatedAt,
                CreatedBy = new AuthorDTO
                {
                    UserId = news.CreatedById,
                    UserName = userName
                },
                File = news.FileId.HasValue ? new FileInfoDTO
                {
                    FileId = news.FileId.Value,
                    FileName = news.File.FileName,
                    DownloadUrl = _fileService.GetFileUrl(news.File, Request)
                } : null
            };
        }

        #endregion

        #endregion
    }

    #endregion
}