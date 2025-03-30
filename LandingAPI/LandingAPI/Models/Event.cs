#region Заголовок файла

/// <summary>
/// Файл: Event.cs
/// Класс, представляющий сущность "Событие" (Event).
/// Используется для хранения данных о событиях, включая их описание, даты, местоположение и связи с другими сущностями.
/// </summary>

#endregion

namespace LandingAPI.Models
{
    #region Класс Event

    /// <summary>
    /// Класс, представляющий сущность "Событие".
    /// </summary>
    public class Event
    {
        #region Свойства

        /// <summary>
        /// Идентификатор события.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Заголовок события.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание события.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата и время начала события.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата и время окончания события.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Местоположение события. Может быть null.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Идентификатор пользователя, создавшего событие.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Пользователь, создавший событие.
        /// </summary>
        public User CreatedBy { get; set; }

        /// <summary>
        /// Идентификатор файла, связанного с событием. Может быть null.
        /// </summary>
        public int? FileId { get; set; }

        /// <summary>
        /// Файл, связанный с событием. Может быть null.
        /// </summary>
        public Files? File { get; set; }

        /// <summary>
        /// Дата и время создания события. По умолчанию устанавливается текущее время в UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }

    #endregion
}