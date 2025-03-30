namespace LandingAPI.DTO.Common
{
    #region Класс FileInfoDTO
    /// <summary>
    /// Информация о прикрепленном файле.
    /// </summary>
    public class FileInfoDTO
    {
        #region Свойства
        /// <summary>
        /// ID файла.
        /// </summary>
        /// <example>12</example>
        public int FileId { get; set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        /// <example>agenda.pdf</example>
        public string FileName { get; set; }

        /// <summary>
        /// Ссылка для скачивания.
        /// </summary>
        /// <example>https://api.example.com/api/Files/download/12</example>
        public string DownloadUrl { get; set; }

        #endregion
    }

    #endregion
}
