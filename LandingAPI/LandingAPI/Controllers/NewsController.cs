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
using LandingAPI.DTO;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Models;
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

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NewsController"/>.
        /// </summary>
        /// <param name="newsRepository">Репозиторий для работы с новостями.</param>
        /// <param name="mapper">Объект для маппинга данных между моделями и DTO.</param>
        public NewsController(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
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
        /// - 200 OK с списком новостей в формате <see cref="NewsDTO"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NewsDTO>), 200)]
        public async Task<IActionResult> GetNewsAsync(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string sort = "NewsId",
            [FromQuery] bool asc = true)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (news, totalCount) = await _newsRepository.GetNewsAsync();
            if (news == null)
                return NotFound();

            var newsDtos = _mapper.Map<List<NewsDTO>>(news);
            return Ok(new { News = newsDtos, TotalCount = totalCount });
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
        /// - 200 OK с данными новости в формате <see cref="NewsDTO"/>.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<NewsDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNewAsync(int id)
        {
            if (!await _newsRepository.NewsExistsAsync(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var news = await _newsRepository.GetNewsAsync(id);
            var newsDtos = _mapper.Map<NewsDTO>(news);

            return Ok(newsDtos);
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
        /// - 200 OK с данными новости в формате <see cref="NewsDTO"/>.
        /// </returns>
        [HttpGet("search/{title}")]
        [ProducesResponseType(typeof(IEnumerable<NewsDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNewByTitleAsync(string title)
        {
            if (!await _newsRepository.NewsExistsByTitleAsync(title))
                return NotFound();

            var news = await _newsRepository.GetNewsByTitleAsync(title);
            var newsDtos = _mapper.Map<NewsDTO>(news);
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
        public async Task<IActionResult> CreateNews([FromBody] NewsDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var news = new News
            {
                Title = model.Title,
                Content = model.Content,
                CreatedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CreatedAt = DateTime.UtcNow,
                FileId = model.FileId
            };

            await _newsRepository.AddNewsAsync(news);
            return Ok(news);
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
        public async Task<IActionResult> UpdateNews(int id, [FromBody] NewsDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var news = await _newsRepository.GetNewsAsync(id);
            if (news == null)
                return NotFound("Новость не найдена.");

            news.Title = model.Title;
            news.Content = model.Content;
            news.FileId = model.FileId;

            await _newsRepository.UpdateNewsAsync(news);
            return Ok(news);
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

        #endregion
    }

    #endregion
}