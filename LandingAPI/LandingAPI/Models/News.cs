#region Заголовок файла

/// <summary>
/// Файл: News.cs
/// Класс, представляющий сущность "Новость" (News).
/// Используется для хранения данных о новостях, включая их заголовок, содержание, автора и связанные файлы.
/// </summary>

#endregion

namespace LandingAPI.Models
{
    #region Класс News

    /// <summary>
    /// Класс, представляющий сущность "Новость".
    /// </summary>
    public class News
    {
        #region Свойства

        /// <summary>
        /// Идентификатор новости.
        /// </summary>
        public int NewsId { get; set; }

        /// <summary>
        /// Заголовок новости.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Содержание новости.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Идентификатор пользователя, создавшего новость.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Пользователь, создавший новость.
        /// </summary>
        public User CreatedBy { get; set; }

        /// <summary>
        /// Идентификатор файла, связанного с новостью. Может быть null.
        /// </summary>
        public int? FileId { get; set; }

        /// <summary>
        /// Файл, связанный с новостью. Может быть null.
        /// </summary>
        public Files? File { get; set; }

        /// <summary>
        /// Дата и время создания новости. По умолчанию устанавливается текущее время в UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }

    #endregion
}