#region Заголовок файла

/// <summary>
/// Файл: Files.cs
/// Класс, представляющий сущность "Файл" (Files).
/// Используется для хранения данных о файлах, включая их имя, путь и время загрузки.
/// </summary>

#endregion

namespace LandingAPI.Models
{
    #region Класс Files

    /// <summary>
    /// Класс, представляющий сущность "Файл".
    /// </summary>
    public class Files
    {
        #region Свойства

        /// <summary>
        /// Идентификатор файла.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Путь к файлу.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Дата и время загрузки файла. По умолчанию устанавливается текущее время в UTC.
        /// </summary>
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }

    #endregion
}