#region Заголовок файла

/// <summary>
/// Файл: EventDTO.cs
/// Класс Data Transfer Object (DTO) для сущности <see cref="Event"/>.
/// Используется для передачи данных о событии между слоями приложения.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using System.ComponentModel.DataAnnotations;

#endregion

namespace LandingAPI.DTO
{
    #region Класс EventDTO

    /// <summary>
    /// Data Transfer Object (DTO) для сущности <see cref="Event"/>.
    /// </summary>
    public class EventDTO
    {
        #region Свойства

        /// <summary>
        /// Идентификатор события.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Заголовок события. Обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Заголовок является обязательным")]
        [MinLength(1, ErrorMessage = "Заголовок не может быть пустым")]
        [MaxLength(100, ErrorMessage = "Заголовок не может превышать 100 символов")]
        public string Title { get; set; }

        /// <summary>
        /// Описание события. Обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Описание является обязательным")]
        [MinLength(1, ErrorMessage = "Описание не может быть пустым")]
        [MaxLength(5000, ErrorMessage = "Описание не может превышать 5000 символов")]
        public string Description { get; set; }

        /// <summary>
        /// Дата и время начала события. Обязательное поле.
        /// </summary>
        public required DateTime StartDate { get; set; }

        /// <summary>
        /// Дата и время окончания события. Обязательное поле.
        /// </summary>
        public required DateTime EndDate { get; set; }

        /// <summary>
        /// Местоположение события. Может быть null.
        /// </summary>
        public required string? Location { get; set; }

        /// <summary>
        /// Идентификатор файла, связанного с событием. Может быть null.
        /// </summary>
        public int? FileId { get; set; }

        /// <summary>
        /// Пользователь, создавший событие.
        /// </summary>
        public User CreatedBy { get; set; }

        /// <summary>
        /// Дата и время создания события. По умолчанию устанавливается текущее время в UTC.
        /// </summary>
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }

    #endregion
}