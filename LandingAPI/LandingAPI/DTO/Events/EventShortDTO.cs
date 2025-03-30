#region Пространство имен

using System;

#endregion

namespace LandingAPI.DTO.Events
{
    #region Класс EventShortDTO
    /// <summary>
    /// Краткая информация о событии
    /// </summary>
    public class EventShortDTO
    {
        #region Свойства
        /// <summary>
        /// ID события
        /// </summary>
        /// <example>1</example>
        public int EventId { get; set; }

        /// <summary>
        /// Название события
        /// </summary>
        /// <example>Техническая конференция</example>
        public string Title { get; set; }

        /// <summary>
        /// Дата и время начала
        /// </summary>
        /// <example>2023-12-01T09:00:00</example>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата и время окончания
        /// </summary>
        /// <example>2023-12-03T18:00:00</example>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Местоположение
        /// </summary>
        /// <example>Москва, Крокус Сити Холл</example>
        public string? Location { get; set; }

        /// <summary>
        /// Имя автора события
        /// </summary>
        /// <example>admin</example>
        public string AuthorName { get; set; }

        /// <summary>
        /// Имеет ли событие прикрепленный файл
        /// </summary>
        /// <example>true</example>
        public bool HasAttachment { get; set; }
        #endregion
    }
    #endregion
}
