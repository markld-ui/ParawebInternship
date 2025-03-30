#region Заголовок файла

/// <summary>
/// Файл: FileService.cs
/// Сервис для работы с файлами. Обеспечивает загрузку, удаление и управление файлами.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using LandingAPI.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

#endregion

namespace LandingAPI.Services
{
    #region Класс FileService

    /// <summary>
    /// Сервис для работы с файлами.
    /// </summary>
    public class FileService
    {
        #region Поля

        private readonly IFilesRepository _filesRepository;
        private readonly string _webRootPath;
        private const string FilesFolder = "files";

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FileService"/>.
        /// </summary>
        /// <param name="filesRepository">Репозиторий для работы с файлами, реализующий интерфейс <see cref="IFilesRepository"/>.</param>
        /// <param name="environment">Объект, представляющий среду веб-хостинга, 
        /// содержащий информацию о директории для загрузки файлов, реализующий интерфейс <see cref="IWebHostEnvironment"/>.</param>
        public FileService(IFilesRepository filesRepository, IWebHostEnvironment environment)
        {
            _filesRepository = filesRepository;
            _webRootPath = environment.WebRootPath;

            var filesDirectory = Path.Combine(_webRootPath, FilesFolder);
            if (!Directory.Exists(filesDirectory))
            {
                Directory.CreateDirectory(filesDirectory);
            }
        }

        #endregion

        #region Методы

        #region UploadFileAsync

        /// <summary>
        /// Загружает файл на сервер и сохраняет информацию о нем в базе данных.
        /// </summary>
        /// <param name="file">Файл для загрузки, представленный интерфейсом <see cref="IFormFile"/>.</param>
        /// <returns>Информация о загруженном файле в виде объекта <see cref="Files"/>.</returns>
        public async Task<Files> UploadFileAsync(IFormFile file)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var relativePath = Path.Combine(FilesFolder, uniqueFileName);
            var filePath = Path.Combine(_webRootPath, relativePath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileEntity = new Files
            {
                FileName = file.FileName,
                FilePath = relativePath,
                UploadedAt = DateTime.UtcNow
            };

            await _filesRepository.AddFileAsync(fileEntity);
            return fileEntity;
        }
        #endregion

        #region DeleteFileAsync

        /// <summary>
        /// Удаляет файл с сервера и из базы данных.
        /// </summary>
        /// <param name="fileId">Идентификатор файла, который необходимо удалить.</param>
        /// <returns>True, если удаление прошло успешно, иначе false.</returns>
        public async Task<bool> DeleteFileAsync(int fileId)
        {
            var file = await _filesRepository.GetFileByIdAsync(fileId);
            if (file == null)
                return false;

            var fullPath = Path.Combine(_webRootPath, file.FilePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            await _filesRepository.DeleteFileAsync(file);
            return true;
        }
        #endregion

        #region GetFileStreamAsync

        /// <summary>
        /// Получает файл по его идентификатору.
        /// </summary>
        /// <param name="fileId">Идентификатор файла, который необходимо получить.</param>
        /// <returns>
        /// Кортеж, содержащий информацию о файле и поток для чтения. 
        /// Если файл не найден или не существует, возвращает (null, null).
        /// </returns>
        public async Task<(Files file, Stream stream)> GetFileStreamAsync(int fileId)
        {
            var file = await _filesRepository.GetFileByIdAsync(fileId);
            if (file == null) return (null, null);

            var fullPath = Path.Combine(_webRootPath, file.FilePath);
            if (!System.IO.File.Exists(fullPath)) return (null, null);

            return (file, new FileStream(fullPath, FileMode.Open, FileAccess.Read));
        }
        #endregion

        #region GetFileUrl

        /// <summary>
        /// Генерирует полный URL для заданного файла на основе его пути и HTTP-запроса.
        /// </summary>
        /// <param name="file">Объект типа <see cref="Files"/>, представляющий файл, для которого необходимо сгенерировать URL.</param>
        /// <param name="request">Объект типа <see cref="HttpRequest"/>, содержащий информацию о текущем HTTP-запросе, включая схему и хост.</param>
        /// <returns>Возвращает строку, представляющую полный URL к файлу. Если файл равен null, возвращает null.</returns>
        public string GetFileUrl(Files file, HttpRequest request)
        {
            return file == null ? null : $"{request.Scheme}://{request.Host}/{file.FilePath.Replace('\\', '/')}";
        }

        #endregion

        #endregion
    }

    #endregion
}