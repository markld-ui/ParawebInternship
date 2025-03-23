#region Заголовок файла

/// <summary>
/// Файл: FilesDTO.cs
/// Класс Data Transfer Object (DTO) для сущности <see cref="Files"/>.
/// Используется для передачи данных о файлах между слоями приложения.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;

#endregion

namespace LandingAPI.DTO
{
    #region Класс FilesDTO

    /// <summary>
    /// Data Transfer Object (DTO) для сущности <see cref="Files"/>.
    /// </summary>
    public class FilesDTO
    {
        #region Свойства

        /// <summary>
        /// Идентификатор файла.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Имя файла. Обязательное поле.
        /// </summary>
        public required string FileName { get; set; }

        /// <summary>
        /// Путь к файлу. Обязательное поле.
        /// </summary>
        public required string FilePath { get; set; }

        /// <summary>
        /// Дата и время загрузки файла. По умолчанию устанавливается текущее время в UTC.
        /// </summary>
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }

    #endregion
}