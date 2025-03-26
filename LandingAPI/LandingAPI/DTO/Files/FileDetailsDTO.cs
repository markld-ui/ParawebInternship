#region Пространство имен

using System;

#endregion

namespace LandingAPI.DTO.Files
{
    #region Класс FileDetailsDTO
    /// <summary>
    /// Data Transfer Object (DTO) для представления деталей файла.
    /// </summary>
    public class FileDetailsDTO
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор файла.
        /// </summary>
        /// <example>1</example>
        public int FileId { get; set; }

        /// <summary>
        /// Название файла, включая расширение.
        /// </summary>
        /// <example>document.pdf</example>
        public string FileName { get; set; }

        /// <summary>
        /// Дата и время загрузки файла.
        /// </summary>
        /// <example>2023-10-01T15:30:00</example>
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// URL для скачивания файла.
        /// </summary>
        /// <example>https://example.com/files/document.pdf</example>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Размер файла в байтах.
        /// </summary>
        /// <example>204800</example>
        public long FileSize { get; set; }
        #endregion
    }
    #endregion
}
