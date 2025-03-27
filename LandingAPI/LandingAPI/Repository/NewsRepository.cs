#region Заголовок файла

/// <summary>
/// Файл: NewsRepository.cs
/// Реализация репозитория для работы с сущностью <see cref="News"/>.
/// Предоставляет методы для доступа к данным о новостях в базе данных.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Data;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

#endregion

namespace LandingAPI.Repository
{
    #region Класс NewsRepository

    /// <summary>
    /// Реализация репозитория для работы с сущностью <see cref="News"/>.
    /// </summary>
    public class NewsRepository : INewsRepository
    {
        #region Поля

        private readonly DataContext _context;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NewsRepository"/>.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public NewsRepository(DataContext context)
        {
            _context = context;
        }

        #endregion

        #region Методы

        #region GetNewsAsync
        /// <summary>
        /// Получает список всех новостей, отсортированных по указанному полю.
        /// </summary>
        /// <param name="pageNumber">Номер страницы для получения (по умолчанию 1).</param>
        /// <param name="pageSize">Количество новостей на странице (по умолчанию 10).</param>
        /// <param name="sortField">Поле, по которому будет производиться сортировка (по умолчанию "NewsId").</param>
        /// <param name="ascending">Определяет порядок сортировки: по возрастанию или убыванию (по умолчанию true).</param>
        /// <returns>Кортеж, содержащий коллекцию новостей и общее количество новостей.</returns>
        public async Task<(ICollection<News> News, int TotalCount)> GetNewsAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortField = "NewsId",
            bool ascending = true)
        {
            var allowedFields = new[] { "NewsId", "Title", "CreatedAt" };
            if (!allowedFields.Contains(sortField))
            {
                sortField = "NewsId";
            }

            var query = _context.News
                .Include(n => n.File)
                .Include(n => n.CreatedBy)
                .AsQueryable();

            string orderDirection = ascending ? "ASC" : "DESC";
            query = query.OrderBy($"{sortField} {orderDirection}");

            var totalCount = await query.CountAsync();
            var news = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (news, totalCount);
        }
        #endregion

        #region GetNewsAsync
        /// <summary>
        /// Получает новость по ее идентификатору.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns>Новость, если найдена, иначе <c>null</c>.</returns>
        public async Task<News> GetNewsAsync(int newsId)
        {
            return await _context.News.Where(n => n.NewsId == newsId).FirstOrDefaultAsync();
        }
        #endregion

        #region GetNewsByTitleAsync
        /// <summary>
        /// Получает новость по ее заголовку (с использованием поиска по частичному совпадению).
        /// </summary>
        /// <param name="title">Заголовок новости.</param>
        /// <returns>Новость, если найдена, иначе <c>null</c>.</returns>
        public async Task<News> GetNewsByTitleAsync(string title)
        {
            return await _context.News
                .Where(n => EF.Functions.Like(n.Title, $"%{title}%"))
                .FirstOrDefaultAsync();
        }
        #endregion

        #region NewsExistsAsync
        /// <summary>
        /// Проверяет существование новости по ее идентификатору.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns>
        /// <c>true</c>, если новость с указанным идентификатором существует, иначе <c>false</c>.
        /// </returns>
        public async Task<bool> NewsExistsAsync(int newsId)
        {
            return await _context.News.AnyAsync(n => n.NewsId == newsId);
        }
        #endregion

        #region NewsExistsByTitleAsync
        /// <summary>
        /// Проверяет существование новости по ее заголовку (с использованием поиска по частичному совпадению).
        /// </summary>
        /// <param name="title">Заголовок новости.</param>
        /// <returns>
        /// <c>true</c>, если новость с указанным заголовком существует, иначе <c>false</c>.
        /// </returns>
        public async Task<bool> NewsExistsByTitleAsync(string title)
        {
            return await _context.News
                .AnyAsync(n => EF.Functions.Like(n.Title, $"%{title}%"));
        }
        #endregion

        #region AddNewsAsync
        /// <summary>
        /// Добавляет новость в базу данных асинхронно.
        /// </summary>
        /// <param name="news">Объект новости, который нужно добавить.</param>
        public async Task AddNewsAsync(News news)
        {
            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region UpdateNewsAsync
        /// <summary>
        /// Обновляет существующую новость в базе данных асинхронно.
        /// </summary>
        /// <param name="news">Объект новости с обновленными данными.</param>
        public async Task UpdateNewsAsync(News news)
        {
            _context.News.Update(news);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DeleteNewsAsync
        /// <summary>
        /// Удаляет новость из базы данных асинхронно.
        /// </summary>
        /// <param name="news">Объект новости, который нужно удалить.</param>
        public async Task DeleteNewsAsync(News news)
        {
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
        }
        #endregion

        #endregion
    }

    #endregion
}