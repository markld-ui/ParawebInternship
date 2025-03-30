#region Заголовок файла

/// <summary>
/// Файл: FilesRepository.cs
/// Реализация репозитория для работы с сущностью <see cref="Files"/>.
/// Предоставляет методы для доступа к данным о файлах в базе данных, включая файлы, связанные с новостями и событиями.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using LandingAPI.Data;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

#endregion

namespace LandingAPI.Repository
{
    #region Класс FilesRepository

    /// <summary>
    /// Реализация репозитория для работы с сущностью <see cref="Files"/>.
    /// </summary>
    public class FilesRepository : IFilesRepository
    {
        #region Поля

        private readonly DataContext _context;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FilesRepository"/>.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public FilesRepository(DataContext context)
        {
            _context = context;
        }

        #endregion

        #region Методы

        #region FileExistsByIdAsync

        /// <summary>
        /// Проверяет существование файла по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <returns>
        /// <c>true</c>, если файл с указанным идентификатором существует, иначе <c>false</c>.
        /// </returns>
        public async Task<bool> FileExistsByIdAsync(int id)
        {
            return await _context.Files.AnyAsync(f => f.FileId == id);
        }
        #endregion

        #region FileExistsByNameAsync

        /// <summary>
        /// Проверяет существование файла по его имени.
        /// </summary>
        /// <param name="name">Имя файла.</param>
        /// <returns>
        /// <c>true</c>, если файл с указанным именем существует, иначе <c>false</c>.
        /// </returns>
        public async Task<bool> FileExistsByNameAsync(string name)
        {
            return await _context.Files.AnyAsync(f => f.FileName == name);
        }
        #endregion

        #region GetFileByIdAsync

        /// <summary>
        /// Получает файл по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <returns>Файл, если найден, иначе <c>null</c>.</returns>
        public async Task<Files> GetFileByIdAsync(int id)
        {
            return await _context.Files.Where(f => f.FileId == id).FirstOrDefaultAsync();
        }
        #endregion

        #region GetFilesAsync
        /// <summary>
        /// Получает список всех файлов, отсортированных по указанному полю.
        /// </summary>
        /// <param name="pageNumber">Номер страницы для получения (по умолчанию 1).</param>
        /// <param name="pageSize">Количество файлов на странице (по умолчанию 10).</param>
        /// <param name="sortField">Поле, по которому будет производиться сортировка (по умолчанию "UploadedAt").</param>
        /// <param name="ascending">Определяет порядок сортировки: по возрастанию или убыванию (по умолчанию false).</param>
        /// <returns>Кортеж, содержащий коллекцию файлов и общее количество файлов.</returns>
        public async Task<(ICollection<Files> Files, int TotalCount)> GetFilesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortField = "UploadedAt",
            bool ascending = false)
        {
            var allowedFields = new[] { "FileId", "UploadedAt" };
            if (!allowedFields.Contains(sortField))
            {
                sortField = "UploadedAt";
            }

            var query = _context.Files.AsQueryable();

            string orderDirection = ascending ? "ASC" : "DESC";
            query = query.OrderBy($"{sortField} {orderDirection}");

            var totalCount = await query.CountAsync();
            var files = await query
                .Skip((pageSize - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (files, totalCount);
        }
        #endregion

        #region AddFileAsync

        /// <summary>
        /// Добавляет файл в базу данных асинхронно.
        /// </summary>
        /// <param name="file">Объект файла, который нужно добавить.</param>
        public async Task AddFileAsync(Files file)
        {
            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DeleteFileAsync
        /// <summary>
        /// Удаляет файл из базы данных асинхронно.
        /// </summary>
        /// <param name="file">Объект файла, который нужно удалить.</param>
        public async Task DeleteFileAsync(Files file)
        {
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Методы для получения файлов, связанных с новостями и событиями

        #region GetFileByNewsIdAsync
        /// <summary>
        /// Получает файл, связанный с определенной новостью.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns>Файл, если найден, иначе <c>null</c>.</returns>
        public async Task<Files> GetFileByNewsIdAsync(int newsId)
        {
            return await _context.News
                .Where(n => n.NewsId == newsId)
                .Select(n => n.File)
                .FirstOrDefaultAsync();
        }
        #endregion

        #region GetFileByEventIdAsync
        /// <summary>
        /// Получает файл, связанный с определенным событием.
        /// </summary>
        /// <param name="eventId">Идентификатор события.</param>
        /// <returns>Файл, если найден, иначе <c>null</c>.</returns>
        public async Task<Files> GetFileByEventIdAsync(int eventId)
        {
            return await _context.Events
                .Where(e => e.EventId == eventId)
                .Select(e => e.File)
                .FirstOrDefaultAsync();
        }
        #endregion

        #endregion

        #endregion
    }

    #endregion
}