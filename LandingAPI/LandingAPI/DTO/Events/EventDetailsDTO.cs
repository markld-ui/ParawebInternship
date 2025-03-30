#region Пространство имен

using LandingAPI.DTO.News;
using LandingAPI.DTO.Common;
using System;

#endregion

namespace LandingAPI.DTO.Events
{
    #region Класс EventDetailsDTO
    /// <summary>
    /// Детальная информация о событии
    /// </summary>
    public class EventDetailsDTO
    {
        #region Свойства
        /// <summary>
        /// ID события
        /// </summary>
        /// <example>5</example>
        public int EventId { get; set; }

        /// <summary>
        /// Название события
        /// </summary>
        /// <example>Техническая конференция</example>
        public string Title { get; set; }

        /// <summary>
        /// Подробное описание
        /// </summary>
        /// <example>Ежегодная конференция для разработчиков</example>
        public string Description { get; set; }

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
        /// Местоположение проведения
        /// </summary>
        /// <example>Москва, Крокус Сити Холл</example>
        public string? Location { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        /// <example>2023-11-15T10:00:00</example>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Автор события
        /// </summary>
        public AuthorDTO CreatedBy { get; set; }

        /// <summary>
        /// Прикрепленный файл (если имеется)
        /// </summary>
        public FileInfoDTO? File { get; set; }
        #endregion
    }
    #endregion
}
