#region Пространство имен

using System;

#endregion

namespace LandingAPI.DTO.News
{
    #region Класс NewsShortDTO
    /// <summary>
    /// Data Transfer Object (DTO) для представления краткой информации о новостной записи.
    /// </summary>
    public class NewsShortDTO
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор новости.
        /// </summary>
        public int NewsId { get; set; }

        /// <summary>
        /// Заголовок новости.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Дата и время создания новости.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Имя автора, создавшего новость.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Указывает, есть ли вложение к новости (например, файл или изображение).
        /// </summary>
        public bool HasAttachment { get; set; }
        #endregion
    }
    #endregion
}
