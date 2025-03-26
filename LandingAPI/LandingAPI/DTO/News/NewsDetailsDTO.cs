#region Пространство имен

using System;
using LandingAPI.DTO.Common;

#endregion

namespace LandingAPI.DTO.News
{
    #region Класс NewsDetailsDTO
    /// <summary>
    /// Data Transfer Object (DTO) для представления деталей новостной записи.
    /// </summary>
    public class NewsDetailsDTO
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
        /// Содержание новости.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Дата и время создания новости.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Автор, создавший новость.
        /// </summary>
        public AuthorDTO CreatedBy { get; set; }

        /// <summary>
        /// Информация о файле, связанном с новостью (например, изображение).
        /// Может быть null, если файл отсутствует.
        /// </summary>
        public FileInfoDTO? File { get; set; }
        #endregion
    }
    #endregion
}
