#region Заголовок файла

/// <summary>
/// Файл: IFilesRepository.cs
/// Интерфейс репозитория для работы с сущностью <see cref="Files"/>.
/// Определяет методы для работы с файлами в базе данных, включая получение файлов, связанных с новостями и событиями.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace LandingAPI.Interfaces.Repositories
{
    #region Интерфейс IFilesRepository

    /// <summary>
    /// Интерфейс репозитория для работы с сущностью <see cref="Files"/>.
    /// </summary>
    public interface IFilesRepository
    {
        #region Методы

        /// <summary>
        /// Получает список всех файлов.
        /// </summary>
        /// <returns>Коллекция файлов.</returns>
        Task<ICollection<Files>> GetFilesAsync();

        /// <summary>
        /// Получает файл по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <returns>Файл, если найден, иначе <c>null</c>.</returns>
        Task<Files> GetFileByIdAsync(int id);

        /// <summary>
        /// Проверяет существование файла по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <returns>
        /// <c>true</c>, если файл с указанным идентификатором существует, иначе <c>false</c>.
        /// </returns>
        Task<bool> FileExistsByIdAsync(int id);

        /// <summary>
        /// Проверяет существование файла по его имени.
        /// </summary>
        /// <param name="name">Имя файла.</param>
        /// <returns>
        /// <c>true</c>, если файл с указанным именем существует, иначе <c>false</c>.
        /// </returns>
        Task<bool> FileExistsByNameAsync(string name);

        #region Методы для получения файлов, связанных с новостями и событиями

        /// <summary>
        /// Получает файл, связанный с определенной новостью.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns>Файл, если найден, иначе <c>null</c>.</returns>
        Task<Files> GetFileByNewsIdAsync(int newsId);

        /// <summary>
        /// Получает файл, связанный с определенным событием.
        /// </summary>
        /// <param name="eventId">Идентификатор события.</param>
        /// <returns>Файл, если найден, иначе <c>null</c>.</returns>
        Task<Files> GetFileByEventIdAsync(int eventId);

        #endregion

        #endregion
    }

    #endregion
}