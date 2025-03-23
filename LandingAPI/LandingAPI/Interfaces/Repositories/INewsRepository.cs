#region Заголовок файла

/// <summary>
/// Файл: INewsRepository.cs
/// Интерфейс репозитория для работы с сущностью <see cref="News"/>.
/// Определяет методы для работы с новостями в базе данных.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace LandingAPI.Interfaces.Repositories
{
    #region Интерфейс INewsRepository

    /// <summary>
    /// Интерфейс репозитория для работы с сущностью <see cref="News"/>.
    /// </summary>
    public interface INewsRepository
    {
        #region Методы

        /// <summary>
        /// Получает список всех новостей.
        /// </summary>
        /// <returns>Коллекция новостей.</returns>
        Task<ICollection<News>> GetNewsAsync();

        /// <summary>
        /// Получает новость по ее идентификатору.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns>Новость, если найдена, иначе <c>null</c>.</returns>
        Task<News> GetNewsAsync(int newsId);

        /// <summary>
        /// Получает новость по ее заголовку.
        /// </summary>
        /// <param name="title">Заголовок новости.</param>
        /// <returns>Новость, если найдена, иначе <c>null</c>.</returns>
        Task<News> GetNewsByTitleAsync(string title);

        /// <summary>
        /// Проверяет существование новости по ее идентификатору.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns>
        /// <c>true</c>, если новость с указанным идентификатором существует, иначе <c>false</c>.
        /// </returns>
        Task<bool> NewsExistsAsync(int newsId);

        /// <summary>
        /// Проверяет существование новости по ее заголовку.
        /// </summary>
        /// <param name="title">Заголовок новости.</param>
        /// <returns>
        /// <c>true</c>, если новость с указанным заголовком существует, иначе <c>false</c>.
        /// </returns>
        Task<bool> NewsExistsByTitleAsync(string title);

        Task AddNewsAsync(News news);
        Task UpdateNewsAsync(News news);
        Task DeleteNewsAsync(News news);

        #endregion
    }

    #endregion
}