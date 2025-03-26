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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

#endregion

namespace LandingAPI.Controllers
{
    #region Класс NewsController

    /// <summary>
    /// Контроллер для управления новостями.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : Controller
    {
        #region Поля и свойства

        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        private readonly IFilesRepository _filesRepository;
        private readonly FileService _fileService;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NewsController"/>.
        /// </summary>
        /// <param name="newsRepository">Репозиторий для работы с новостями.</param>
        /// <param name="mapper">Объект для маппинга данных между моделями и DTO.</param>
        /// <param name="filesRepository">Репозиторий для работы с файлами.</param>
        public NewsController(INewsRepository newsRepository, IMapper mapper, IFilesRepository filesRepository, FileService fileService)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _filesRepository = filesRepository;
            _fileService = fileService;
        }

        #endregion

        #region Методы

        #region GetNewsAsync

        /// <summary>
        /// Получает список всех новостей.
        /// </summary>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если новости не найдены.
        /// - 200 OK с списком новостей в формате <see cref="NewsShortDTO"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<NewsShortDTO>), 200)]
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
        /// - 404 NotFound, если новость с указанным идентификатором не найдена.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с данными новости в формате <see cref="NewsDetailsDTO"/>.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NewsDetailsDTO), 200)]
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
        /// - 404 NotFound, если новость с указанным заголовком не найдена.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с данными новости в формате <see cref="NewsDetailsDTO"/>.
        /// </returns>
        [HttpGet("search/{title}")]
        [ProducesResponseType(typeof(IEnumerable<NewsDetailsDTO>), 200)]
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
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с данными созданной новости.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// </remarks>
        [Authorize(Roles = "Admin")]
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
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если новость с указанным идентификатором не найдена.
        /// - 200 OK с данными обновленной новости.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// </remarks>
        [Authorize(Roles = "Admin")]
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
        /// - 404 NotFound, если новость с указанным идентификатором не найдена.
        /// - 204 NoContent, если новость успешно удалена.
        /// </returns>
        /// <remarks>
        /// Доступно только для пользователей с ролью "Admin".
        /// </remarks>
        [Authorize(Roles = "Admin")]
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
        /// </remarks>
        private async Task<NewsDetailsDTO> MapToDetailsDTO(News news)
        {
            return new NewsDetailsDTO
            {
                NewsId = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                CreatedAt = news.CreatedAt,
                CreatedBy = new AuthorDTO
                {
                    UserId = news.CreatedById,
                    UserName = news.CreatedBy.Username
                },
                File = news.FileId.HasValue ? new FileInfoDTO
                {
                    FileId = news.FileId.Value,
                    FileName = news.File.FileName,
                    DownloadUrl = Url.Action("DownloadFile", "Files", new { id = news.FileId })
                } : null
            };
        }
        #endregion

        #endregion
    }

    #endregion
}