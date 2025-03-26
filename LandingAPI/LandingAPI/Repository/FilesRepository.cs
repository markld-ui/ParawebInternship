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

        /// <summary>
        /// Получает файл по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <returns>Файл, если найден, иначе <c>null</c>.</returns>
        public async Task<Files> GetFileByIdAsync(int id)
        {
            return await _context.Files.Where(f => f.FileId == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Получает список всех файлов, отсортированных по идентификатору.
        /// </summary>
        /// <returns>Коллекция файлов.</returns>
        public async Task<(ICollection<Files> Files, int TotalCount)> GetFilesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortField = "UploadedAt",
            bool ascending = false)
        {
            var query = _context.Files.AsQueryable();

            query = sortField switch
            {
                "FileName" => ascending ? query.OrderBy(f => f.FileName) : query.OrderByDescending(f => f.FileName),
                "FileSize" => ascending ? query.OrderBy(f => new FileInfo(f.FilePath).Length)
                                 : query.OrderByDescending(f => new FileInfo(f.FilePath).Length),
                _ => ascending ? query.OrderBy(f => f.UploadedAt) : query.OrderByDescending(f => f.UploadedAt)
            };

            var totalCount = await query.CountAsync();
            var files = await query
                .Skip((pageSize - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (files, totalCount);
        }

        public async Task AddFileAsync(Files file)
        {
            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(Files file)
        {
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }

        #region Методы для получения файлов, связанных с новостями и событиями

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
    }

    #endregion
}