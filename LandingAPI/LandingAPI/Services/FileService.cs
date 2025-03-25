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
        private readonly string _uploadDirectory;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FileService"/>.
        /// </summary>
        /// <param name="filesRepository">Репозиторий для работы с файлами.</param>
        /// <param name="uploadDirectory">Директория для загрузки файлов.</param>
        public FileService(IFilesRepository filesRepository, string uploadDirectory)
        {
            _filesRepository = filesRepository;
            _uploadDirectory = uploadDirectory;

            // Создаем директорию, если она не существует
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        #endregion

        #region Методы

        /// <summary>
        /// Загружает файл на сервер и сохраняет информацию о нем в базе данных.
        /// </summary>
        /// <param name="file">Файл для загрузки.</param>
        /// <returns>Информация о загруженном файле.</returns>
        public async Task<Files> UploadFileAsync(IFormFile file)
        {
            // Генерируем уникальное имя файла
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadDirectory, uniqueFileName);

            // Сохраняем файл на диск
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Создаем запись о файле в базе данных
            var fileEntity = new Files
            {
                FileName = file.FileName,
                FilePath = filePath,
                UploadedAt = DateTime.UtcNow
            };

            await _filesRepository.AddFileAsync(fileEntity);

            return fileEntity;
        }

        /// <summary>
        /// Удаляет файл с сервера и из базы данных.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>True, если удаление прошло успешно, иначе false.</returns>
        public async Task<bool> DeleteFileAsync(int fileId)
        {
            var file = await _filesRepository.GetFileByIdAsync(fileId);
            if (file == null)
                return false;

            // Удаляем файл с диска
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }

            // Удаляем запись из базы данных
            await _filesRepository.DeleteFileAsync(file);

            return true;
        }

        /// <summary>
        /// Получает файл по его идентификатору.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>Информация о файле и поток для чтения.</returns>
        public async Task<(Files file, Stream stream)> GetFileStreamAsync(int fileId)
        {
            var file = await _filesRepository.GetFileByIdAsync(fileId);
            if (file == null || !System.IO.File.Exists(file.FilePath))
                return (null, null);

            var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read);
            return (file, stream);
        }

        #endregion
    }

    #endregion
}