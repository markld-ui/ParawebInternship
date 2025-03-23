#region Заголовок файла

/// <summary>
/// Файл: NewsDTO.cs
/// Класс Data Transfer Object (DTO) для сущности <see cref="News"/>.
/// Используется для передачи данных о новостях между слоями приложения.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;

#endregion

namespace LandingAPI.DTO
{
    #region Класс NewsDTO

    /// <summary>
    /// Data Transfer Object (DTO) для сущности <see cref="News"/>.
    /// </summary>
    public class NewsDTO
    {
        #region Свойства

        /// <summary>
        /// Идентификатор новости.
        /// </summary>
        public int NewsId { get; set; }

        /// <summary>
        /// Заголовок новости. Обязательное поле.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Содержание новости. Обязательное поле.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Дата и время создания новости. По умолчанию устанавливается текущее время в UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Пользователь, создавший новость.
        /// </summary>
        public User CreatedBy { get; set; }

        #endregion
    }

    #endregion
}