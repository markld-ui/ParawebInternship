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

        /// <summary>
        /// Получает список всех новостей, отсортированных по идентификатору.
        /// </summary>
        /// <returns>Коллекция новостей.</returns>
        public async Task<ICollection<News>> GetNewsAsync()
        {
            return await _context.News.OrderBy(n => n.NewsId).ToListAsync();
        }

        /// <summary>
        /// Получает новость по ее идентификатору.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns>Новость, если найдена, иначе <c>null</c>.</returns>
        public async Task<News> GetNewsAsync(int newsId)
        {
            return await _context.News.Where(n => n.NewsId == newsId).FirstOrDefaultAsync();
        }

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
    }

    #endregion
}