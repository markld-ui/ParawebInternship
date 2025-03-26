#region Пространство имен

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace LandingAPI.DTO.Events
{
    #region Класс UpdateEventDTO
    /// <summary>
    /// Data Transfer Object (DTO) для обновления сущности <see cref="Event"/>.
    /// </summary>
    public class UpdateEventDTO
    {
        #region Свойства
        /// <summary>
        /// Заголовок события. Максимальная длина 100 символов.
        /// </summary>
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        /// <example>Обновленная конференция по разработке ПО</example>
        public string? Title { get; set; }

        /// <summary>
        /// Описание события. Максимальная длина 5000 символов.
        /// </summary>
        [StringLength(5000, ErrorMessage = "Описание не должно превышать 5000 символов")]
        /// <example>Это обновленное описание для годовой конференции.</example>
        public string? Description { get; set; }

        /// <summary>
        /// Дата и время начала события. Может быть null.
        /// </summary>
        /// <example>2023-10-01T10:00:00</example>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата и время окончания события. Может быть null.
        /// </summary>
        /// <example>2023-10-01T18:00:00</example>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Местоположение события. Может быть null.
        /// </summary>
        /// <example>Обновленный городской конференц-центр</example>
        public string? Location { get; set; }

        /// <summary>
        /// Идентификатор файла, связанного с событием. Может быть null.
        /// </summary>
        /// <example>42</example>
        public int? FileId { get; set; }

        /// <summary>
        /// Указывает, нужно ли удалить связанный файл.
        /// </summary>
        /// <example>true</example>
        public bool? RemoveFile { get; set; }
        #endregion
    }
    #endregion
}
