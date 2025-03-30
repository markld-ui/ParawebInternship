#region Заголовок файла

/// <summary>
/// Файл: NewsDTO.cs
/// Класс Data Transfer Object (DTO) для сущности <see cref="News"/>.
/// Используется для передачи данных о новостях между слоями приложения.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using System.ComponentModel.DataAnnotations;

#endregion

namespace LandingAPI.DTO.News
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
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        public required string Title { get; set; }

        /// <summary>
        /// Содержание новости. Обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Содержание обязательно")]
        [StringLength(5000, ErrorMessage = "Содержание не должно превышать 5000 символов")]
        public required string Content { get; set; }

        /// <summary>
        /// Идентификатор файла, связанного с новостью. Может быть null.
        /// </summary>
        public int? FileId { get; set; }

        /// <summary>
        /// Дата и время создания новости. По умолчанию устанавливается текущее время в UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Пользователь, создавший новость.
        /// </summary>
        public User CreatedBy { get; set; }

        /// <summary>
        /// Свойство, для загрузки файла.
        /// </summary>
        public IFormFile? File { get; set; }

        #endregion
    }

    #endregion
}