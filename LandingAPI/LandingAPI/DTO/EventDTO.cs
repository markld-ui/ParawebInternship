#region Заголовок файла

/// <summary>
/// Файл: EventDTO.cs
/// Класс Data Transfer Object (DTO) для сущности <see cref="Event"/>.
/// Используется для передачи данных о событии между слоями приложения.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;

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
        public required string Title { get; set; }

        /// <summary>
        /// Описание события. Обязательное поле.
        /// </summary>
        public required string Description { get; set; }

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