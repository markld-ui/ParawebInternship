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
        /// <param name="pageNumber">Номер страницы для получения (по умолчанию 1).</param>
        /// <param name="pageSize">Количество файлов на странице (по умолчанию 10).</param>
        /// <param name="sortField">Поле, по которому будет выполнена сортировка (по умолчанию "UploadedAt").</param>
        /// <param name="ascending">Указывает, должна ли сортировка быть по возрастанию (по умолчанию true).</param>
        /// <returns>Кортеж, содержащий коллекцию файлов и общее количество файлов.</returns>
        Task<(ICollection<Files> Files, int TotalCount)> GetFilesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortField = "UploadedAt",
            bool ascending = false);

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

        /// <summary>
        /// Добавляет новый файл в базу данных.
        /// </summary>
        /// <param name="file">Файл для добавления.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task AddFileAsync(Files file);

        /// <summary>
        /// Удаляет файл из базы данных.
        /// </summary>
        /// <param name="file">Файл для удаления.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task DeleteFileAsync(Files file);

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